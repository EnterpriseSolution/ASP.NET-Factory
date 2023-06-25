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
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Junxian.Magic.Win32;
using Junxian.Magic.Common;

namespace Junxian.Magic.Controls
{
	/// <summary>
	/// Base class for handling popup windows.
	/// </summary>
	public class PopupBase : System.Windows.Forms.Form
	{
		// Instance fields
		private PopupShadow _shadow;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Initialize a new instance of the PopupBase class.
		/// </summary>
		public PopupBase()
		{
			InitializeComponent();
			InternalConstruct(VisualStyle.Office2003);
		}

		/// <summary>
		/// Initialize a new instance of the PopupBase class. 
		/// </summary>
		/// <param name="style">Required visual style.</param>
		public PopupBase(VisualStyle style)
		{
			InitializeComponent();
			InternalConstruct(style);
		}

		private void InternalConstruct(VisualStyle style)
		{
			// We only need a shadow in IDE or Office styles
			if ((style == VisualStyle.IDE) || 
				(style == VisualStyle.IDE2005) ||
				(style == VisualStyle.Office2003))
				_shadow = new PopupShadow();
		}

		/// <summary>
		/// Gets the shadow implementation class.
		/// </summary>
		public PopupShadow PopupShadow
		{
			get { return _shadow; }
		}

		/// <summary>
		/// Make the popup visible but without taking the focus
		/// </summary>
		public virtual void ShowWithoutFocus()
		{
			// If we have an attached shadow window
			if (_shadow != null)
			{
				// Show it now without taking the focus
				_shadow.ShowWithoutFocus();
			}

			// Show the window without activating it (i.e. do not take focus)
			User32.ShowWindow(this.Handle, (short)Win32.ShowWindowStyles.SW_SHOWNOACTIVATE);
		}

		/// <summary>
		/// Gets the creation parameters.
		/// </summary>
		protected override CreateParams CreateParams 
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.Style = unchecked((int)(uint)Win32.WindowStyles.WS_POPUP);
				cp.ExStyle = (int)WindowExStyles.WS_EX_TOPMOST + 
							 (int)WindowExStyles.WS_EX_TOOLWINDOW;
				return cp;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				// Must remove the shadow gracefully
				if (_shadow != null)
					_shadow.Dispose();

				if(components != null)
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// PopupBase
			// 
			this.ClientSize = new System.Drawing.Size(100, 100);
			this.ControlBox = false;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PopupBase";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "PopupBase";
			this.Resize += new System.EventHandler(this.OnResize);
			this.Move += new System.EventHandler(this.OnMove);

		}
		#endregion

		private void OnMove(object sender, System.EventArgs e)
		{
			// If we have an attached shadow window
			if (_shadow != null)
			{
				// Move shadow to matching position
				_shadow.ShowRect = RectangleToScreen(ClientRectangle);
			}
		}

		private void OnResize(object sender, System.EventArgs e)
		{
			// If we have an attached shadow window
			if (_shadow != null)
			{
				// Move shadow to matching position
				_shadow.ShowRect = RectangleToScreen(ClientRectangle);
			}
		}
	}
}
