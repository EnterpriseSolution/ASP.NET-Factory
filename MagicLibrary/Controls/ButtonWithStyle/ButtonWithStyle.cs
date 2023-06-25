// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2005. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 3.0.2.0 	www.crownwood.net
// *****************************************************************************

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32;
using Junxian.Magic.Common;
using Junxian.Magic.Controls.Command;

namespace Junxian.Magic.Controls
{
	/// <summary>
	/// Styled button class that does not take the focus.
	/// </summary>
	[ToolboxBitmap(typeof(ButtonWithStyle))]
	[DefaultProperty("Style")]
	public class ButtonWithStyle : Control
	{
		// Instance fields - Exposed properties
		private bool _pushed;
		private bool _staticIDE;
		private bool _alwaysDrawEnabled;
		private bool _alwaysDrawBorder;
		private TextEdge _textEdge;
		private CommandImage _image;
		private LayoutDirection _direction;
		private ButtonStyle _buttonStyle;
		private ImageAttributes _imageAttr;

		// Instance fields - Internal state
		private bool _mouseOver;
		private bool _mouseCapture;
		private bool _office2003GradBack;
		private bool _ide2005GradBack;
		private MouseButtons _mouseButton;
		private ColorDetails _colorDetails;
		
		/// <summary>
		/// Initializes a new instance of the ButtonWithStyle class.
		/// </summary>
		public ButtonWithStyle()
		{
			// NAG processing
			NAG.NAG_Start();

			// Prevent drawing flicker by blitting from memory in WM_PAINT
			SetStyle(ControlStyles.DoubleBuffer | 
					 ControlStyles.AllPaintingInWmPaint |
					 ControlStyles.UserPaint |
					 ControlStyles.ResizeRedraw, true);

			// Prevent base class from trying to generate double click events and
			// so testing clicks against the double click time and rectangle. Getting
			// rid of this allows the user to press then button very quickly.
			SetStyle(ControlStyles.StandardDoubleClick, false);

			// Should not be allowed to select this control
			SetStyle(ControlStyles.Selectable, false);

			// We do not want a mouse up to always cause a click, we decide that
			SetStyle(ControlStyles.StandardClick, false);

			// We are happy to allow a transparent background color
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			// Set initial state
			_image = new CommandImage();
			_mouseOver = false;
			_staticIDE = false;
			_mouseCapture = false;
			_mouseButton = MouseButtons.None;
			_colorDetails = new ColorDetails();

			// Update with the tracking colors for the IDE style
			_colorDetails.DefineTrackColors(SystemColors.Highlight);

			// We need to know when our text changes
			TextChanged += new EventHandler(OnTextChanged);

			// Reset all properties to default values
			ResetImage();
			ResetStyle();
			ResetDirection();
			ResetTextEdge();
			ResetButtonStyle();
			ResetPushed();
			ResetBackColor();
			ResetAlwaysDrawEnabled();
			ResetAlwaysDrawBorder();
			ResetOffice2003GradBack();
			ResetIDE2005GradBack();
		}

		/// <summary>
		/// Dispose of instance resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Make sure we dispose of any associated image
				if (Image != null)
				{
					Image.Dispose();
					Image = null;
				}

				// Color details has resources that need releasing
				_colorDetails.Dispose();
			}
			
			base.Dispose (disposing);
		}

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		public new Color BackColor
		{
			get { return base.BackColor; }
			
			set
			{
				// Tell base class to store the new colour
				base.BackColor = value;
				
				// Generate colours to use based on new backcolor
				_colorDetails.DefineBaseColors(value);
				
				// Tell the details handler the background color is not defaulted
				_colorDetails.DefaultBaseColor = (value == Color.Transparent);
			}
		}
		
		/// <summary>
		/// Resets the Direction property to its default value.
		/// </summary>
		public new void ResetBackColor()
		{
			// Define the new background color
			base.ResetBackColor();
			
			// Tell detail handler that the background is defaulted
			_colorDetails.DefaultBaseColor = true;
		}

		/// <summary>
		/// Gets or sets the image to use for display.
		/// </summary>
		[Category("Appearance")]
		[Description("Specifies the image to associate with button.")]
		[DefaultValue(null)]
		public Image Image
		{
			get { return _image.Image; }

			set
			{
				if (_image.Image != value)
				{
					_image.Image = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Reset the Image property to its default.
		/// </summary>
		public void ResetImage()
		{
			Image = null;
		}

		/// <summary>
		/// Gets or sets the visual style of the control.
		/// </summary>
		[Category("Appearance")]
		[Description("Indicates which visual style to draw with.")]
		[DefaultValue(typeof(VisualStyle), "Office2003")]
		public VisualStyle Style
		{
			get { return _colorDetails.Style; }

			set
			{
				if (_colorDetails.Style != value)
				{
					_colorDetails.Style = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the Style property to its default value.
		/// </summary>
		public void ResetStyle()
		{
			Style = VisualStyle.Office2003;
		}

		/// <summary>
		/// Gets or sets the direction of commands.
		/// </summary>
		[Category("Appearance")]
		[Description("Determines which direction text is drawn in.")]
		[DefaultValue(typeof(LayoutDirection), "Horizontal")]
		public LayoutDirection Direction
		{
			get { return _direction; }

			set
			{
				if (_direction != value)
				{
					_direction = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the Direction property to its default value.
		/// </summary>
		public void ResetDirection()
		{
			Direction = LayoutDirection.Horizontal;
		}

		/// <summary>
		/// Gets or sets the text edge used for commands displaying text.
		/// </summary>
		[Category("Appearance")]
		[Description("Determines where text is drawn relative to any image.")]
		[DefaultValue(typeof(TextEdge), "Right")]
		public TextEdge TextEdge
		{
			get { return _textEdge; }

			set
			{
				if (_textEdge != value)
				{
					_textEdge = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the TextEdge property to its default value.
		/// </summary>
		public void ResetTextEdge()
		{
			TextEdge = TextEdge.Right;
		}

		/// <summary>
		/// Gets and sets the button style.
		/// </summary>
		[Category("Behavior")]
		[Description("Determine how the button operates.")]
		[DefaultValue(typeof(ButtonStyle), "PushButton")]
		public ButtonStyle ButtonStyle
		{
			get { return _buttonStyle; }
			
			set
			{
				if (_buttonStyle != value)
				{
					_buttonStyle = value;
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the ButtonStyle property.
		/// </summary>
		public void ResetButtonStyle()
		{
			ButtonStyle = ButtonStyle.PushButton;
		}
		
		/// <summary>
		/// Gets and sets the pushed state of the button.
		/// </summary>
		[Category("Behavior")]
		[Description("Indicates if the button is pushed or not.")]
		[DefaultValue(false)]
		public bool Pushed
		{
			get { return _pushed; }
			
			set
			{
				if (_pushed != value)
				{
					_pushed = value;
					Invalidate();
				}
			}
		}
				
		/// <summary>
		/// Resets the Pushed property.
		/// </summary>
		public void ResetPushed()
		{
			Pushed = false;
		}
		
		/// <summary>
		/// Gets or sets the extra attributes applied to drawing images.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(null)]
		public ImageAttributes ImageAttributes
		{
			get { return _imageAttr; }
			
			set
			{
				if (_imageAttr != value)
				{
					_imageAttr = value;
					Invalidate();
				}
			}
		}
		
		/// <summary>
		/// Resets the ImageAttributes property.
		/// </summary>
		public void ResetImageAttributes()
		{
			ImageAttributes = null;
		}

		/// <summary>
		/// Gets and sets a value indicating if the IDE style should raise images.
		/// </summary>
		[Category("Appearance")]
		[Description("Indicates if the IDE style should raise the image.")]
		[DefaultValue(false)]
		public bool StaticIDE
		{
			get { return _staticIDE; }
			
			set
			{
				if (_staticIDE != value)
				{
					_staticIDE = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the StaticIDE property to its default value.
		/// </summary>
		public void ResetStaticIDE()
		{
			StaticIDE = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if the button should always drawn in enabled state.
		/// </summary>
		[Category("Appearance")]
		[Description("Indicates if the button should always drawn in enabled state.")]
		[DefaultValue(false)]
		public bool AlwaysDrawEnabled
		{
			get { return _alwaysDrawEnabled; }
			
			set
			{
				if (_alwaysDrawEnabled != value)
				{
					_alwaysDrawEnabled = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the AlwaysDrawEnabled property to its default value.
		/// </summary>
		public void ResetAlwaysDrawEnabled()
		{
			AlwaysDrawEnabled = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if the button should always have a border drawn.
		/// </summary>
		[Category("Appearance")]
		[Description("Indicates if the button should always have a border drawn.")]
		[DefaultValue(false)]
		public bool AlwaysDrawBorder
		{
			get { return _alwaysDrawBorder; }
			
			set
			{
				if (_alwaysDrawBorder != value)
				{
					_alwaysDrawBorder = value;
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the AlwaysDrawBorder property to its default value.
		/// </summary>
		public void ResetAlwaysDrawBorder()
		{
			AlwaysDrawBorder = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if the background should be drawn with a gradient when using Office2003 style.
		/// </summary>
		[Category("Appearance")]
		[Description("Should the background be drawn with a gradient when using Office2003 style.")]
		[DefaultValue(true)]
		public bool Office2003GradBack
		{
			get { return _office2003GradBack; }
			
			set 
			{ 
				if (_office2003GradBack != value)
				{
					_office2003GradBack = value; 
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the Office2003GradBack property to its default value.
		/// </summary>
		public void ResetOffice2003GradBack()
		{
			Office2003GradBack = true;
		}

		/// <summary>
		/// Gets and sets a value indicating if the background should be drawn with a gradient when using IDE2005 style.
		/// </summary>
		[Category("Appearance")]
		[Description("Should the background be drawn with a gradient when using IDE2005 style.")]
		[DefaultValue(false)]
		public bool IDE2005GradBack
		{
			get { return _ide2005GradBack; }
			
			set 
			{ 
				if (_ide2005GradBack != value)
				{
					_ide2005GradBack = value; 
					Invalidate();
				}
			}
		}

		/// <summary>
		/// Resets the IDE2005GradBack property to its default value.
		/// </summary>
		public void ResetIDE2005GradBack()
		{
			IDE2005GradBack = false;
		}

		/// <summary>
		/// Generates a Click event for a button style command.
		/// </summary>
		public void PerformClick()
		{
			OnClick(EventArgs.Empty);
		}

		/// <summary>
		/// Reset the internal state of the mouse handler.
		/// </summary>
		public void ResetMouseState()
		{
			// Reset to assuming no mouse state
			_mouseOver = false;
			_mouseCapture = false;
			_mouseButton = MouseButtons.None;
			
			// Do not need to have mouse captured
			Capture = false;
			
			// Redraw to show any visual changes.
			Refresh();
		}
		
		/// <summary>
		/// Raises the MouseDown event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!_mouseCapture)
			{
				// Only the left mouse button presses the button
				if (e.Button == MouseButtons.Left)
				{
					// Mouse is over the button and being pressed, so enter the button depressed 
					// state and also remember the original button that was pressed. As we only 
					// generate an event when the same button is released.
					_mouseOver = true;
					_mouseCapture = true;
					_mouseButton = e.Button;

					// Redraw to show button state
					Refresh();
				}
			}

			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the MouseUp event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Are we waiting for this button to go up?
			if (e.Button == _mouseButton)
			{
				// Set state back to become normal
				_mouseOver = false;
				_mouseCapture = false;
				
				// Raise the click
				PerformClick();

				// Redraw to show button state
				Refresh();
			}
			else
			{
				// We don't want to lose capture of mouse
				Capture = true;
			}

			base.OnMouseUp(e);
		}

		/// <summary>
		/// Raises the MouseMove event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Is mouse point inside our client rectangle
			bool over = this.ClientRectangle.Contains(new Point(e.X, e.Y));

			// If entering the button area or leaving the button area...
			if (over != _mouseOver)
			{
				// Update state
				_mouseOver = over;

				// Redraw to show button state
				Refresh();
			}

			base.OnMouseMove(e);
		}

		/// <summary>
		/// Raises the MouseEnter event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			// Update state to reflect mouse over the button area
			_mouseOver = true;

			// Redraw to show button state
			Refresh();

			base.OnMouseEnter(e);
		}

		/// <summary>
		/// Raises the MouseLeave event.
		/// </summary>
		/// <param name="e">A MouseEventArgs that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			// Update state to reflect mouse not over the button area
			_mouseOver = false;

			// Redraw to show button state
			Refresh();

			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Raises the SystemColorsChanged event.
		/// </summary>
		/// <param name="e">An EventArgs that contains the event data. </param>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			// Update with the colors for the IDE style
			_colorDetails.DefineBaseColors(BackColor);
			_colorDetails.DefineTrackColors(SystemColors.Highlight);

			// Update the display now
			Refresh();

			base.OnSystemColorsChanged(e);
		}
		
		/// <summary>
		/// Raises the PaintBackground event.
		/// </summary>
		/// <param name="pevent">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if (_colorDetails.DefaultBaseColor && (_colorDetails.Style == VisualStyle.Office2003))
			{
				CommandDraw.DrawGradientBackground(pevent.Graphics, 
												   this, 
												   _colorDetails.BaseColor, 
												   _colorDetails.BaseColor1, 
												   _colorDetails.BaseColor2,
												    _office2003GradBack);
			}
			else if (_colorDetails.DefaultBaseColor && (_colorDetails.Style == VisualStyle.IDE2005))
			{
				CommandDraw.DrawGradientBackground(pevent.Graphics, 
												   this, 
												   _colorDetails.BaseColor, 
												   _colorDetails.BaseColor1, 
												   _colorDetails.BaseColor2,
												   _ide2005GradBack);
			}
			else
				base.OnPaintBackground(pevent);
		}

		/// <summary>
		/// Raises the Paint event.
		/// </summary>
		/// <param name="e">A PaintEventArgs that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Calculate the correct drawing state
			ItemState state;

			// If the mouse is over the button itself
			if (_mouseOver)
			{
				if (_mouseCapture)
					state = ItemState.Pressed;
				else
					state = ItemState.HotTrack;
			}
			else
			{
				if (_mouseCapture)
					state = ItemState.HotTrack;
				else
					state = ItemState.Normal;
			}

			// Draw the button using helper function
			CommandDraw.DrawButtonCommand(e.Graphics, _colorDetails.Style, _direction, 
										  ClientRectangle,	state, (AlwaysDrawEnabled ? true : Enabled), 
										  _textEdge, Font, ForeColor, _colorDetails.BaseColor, 
										  Text, _image, _imageAttr,
										  _colorDetails.TrackBaseColor1, _colorDetails.TrackBaseColor2,
										  _colorDetails.TrackLightColor1, _colorDetails.TrackLightColor2,
										  _colorDetails.TrackLightLightColor1, _colorDetails.TrackLightLightColor2,
										  _colorDetails.TrackDarkColor, _buttonStyle, _pushed, _staticIDE,
										  AlwaysDrawBorder);

			base.OnPaint(e);
		}

		/// <summary>
		/// Raises the Click event.
		/// </summary>
		protected override void OnClick(EventArgs e)
		{
			// If we are a toggle button
			if (_buttonStyle == ButtonStyle.ToggleButton)
			{
				// Then invert our current pushed state
				Pushed = !Pushed;
			}

			// Raise the click event
			base.OnClick(e);
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			// Update to reflect change.
			Invalidate();
		}
	}
}
