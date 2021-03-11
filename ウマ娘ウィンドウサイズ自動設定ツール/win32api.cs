using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ウマ娘ウィンドウサイズ自動設定ツール
{
 	internal class Win32api
	{
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

		/// <summary>
		///     The MoveWindow function changes the position and dimensions of the specified window. For a top-level window, the
		///     position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative
		///     to the upper-left corner of the parent window's client area.
		///     <para>
		///     Go to https://msdn.microsoft.com/en-us/library/windows/desktop/ms633534%28v=vs.85%29.aspx for more
		///     information
		///     </para>
		/// </summary>
		/// <param name="hWnd">C++ ( hWnd [in]. Type: HWND )<br /> Handle to the window.</param>
		/// <param name="X">C++ ( X [in]. Type: int )<br />Specifies the new position of the left side of the window.</param>
		/// <param name="Y">C++ ( Y [in]. Type: int )<br /> Specifies the new position of the top of the window.</param>
		/// <param name="nWidth">C++ ( nWidth [in]. Type: int )<br />Specifies the new width of the window.</param>
		/// <param name="nHeight">C++ ( nHeight [in]. Type: int )<br />Specifies the new height of the window.</param>
		/// <param name="bRepaint">
		///     C++ ( bRepaint [in]. Type: bool )<br />Specifies whether the window is to be repainted. If this
		///     parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This
		///     applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the
		///     parent window uncovered as a result of moving a child window.
		/// </param>
		/// <returns>
		///     If the function succeeds, the return value is nonzero.<br /> If the function fails, the return value is zero.
		///     <br />To get extended error information, call GetLastError.
		/// </returns>
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left, Top, Right, Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public int X
		{
			get { return Left; }
			set { Right -= (Left - value); Left = value; }
		}

		public int Y
		{
			get { return Top; }
			set { Bottom -= (Top - value); Top = value; }
		}

		public int Height
		{
			get { return Bottom - Top; }
			set { Bottom = value + Top; }
		}

		public int Width
		{
			get { return Right - Left; }
			set { Right = value + Left; }
		}

		public static bool operator ==(RECT r1, RECT r2)
		{
			return r1.Equals(r2);
		}

		public static bool operator !=(RECT r1, RECT r2)
		{
			return !r1.Equals(r2);
		}

		public bool Equals(RECT r)
		{
			return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
		}

		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
		}
	}
}
