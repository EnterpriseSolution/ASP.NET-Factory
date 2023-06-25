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
using System.Windows.Forms;
using Junxian.Magic.Win32;

namespace Junxian.Magic.Common
{
	/// <summary>
	/// Helper class for accessing keyboard state
	/// </summary>
    public sealed class KeyHelper
    {
		/// <summary>
		/// Gets a value indicating if the CTRL key is pressed.
		/// </summary>
		public static bool CTRLPressed
		{
			get
			{
				return ((int)(User32.GetKeyState((int)Win32.VirtualKeys.VK_CONTROL) & 0x00008000) != 0);
			}
		}

		/// <summary>
		/// Gets a value indicating if the SHIFT key is pressed.
		/// </summary>
		public static bool SHIFTPressed
		{
			get
			{
				return ((int)(User32.GetKeyState((int)Win32.VirtualKeys.VK_SHIFT) & 0x00008000) != 0);
			}
		}

		/// <summary>
		/// Gets a value indicating if the ALT key is pressed.
		/// </summary>
		public static bool ALTPressed
		{
			get
			{
				return ((int)(User32.GetKeyState((int)Win32.VirtualKeys.VK_MENU) & 0x00008000) != 0);
			}
		}
	}
}


