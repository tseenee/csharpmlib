using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
namespace MLib
{
    public static class Windows
    {

        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        #region Win API
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, uint action);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("User32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string _ClassName, string _WindowName);

        [DllImport("User32.dll")]
        private static extern Int32 SetForegroundWindow(int hWnd);

        [DllImport("user32", EntryPoint = "SetWindowText")]
        private static extern int SetWindowTextA(int hwnd, string lpString);

        [DllImport("User32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);


        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        private static extern bool ReleaseCapture();

        #endregion

        /// <summary>
        /// Focuses a window
        /// </summary>
        /// <param name="Handle">Window to be focused</param>
        static public void Focus(IntPtr Handle)
        {
            SetForegroundWindow(Handle);
        }

        /// <summary>
        /// Minimizes a window
        /// </summary>
        /// <param name="Handle">Window to be minimied</param>
        static public void MinimizeWindow(IntPtr Handle)
        {
            ShowWindow(Handle, 6);
        }

        /// <summary>
        /// Restores a window
        /// </summary>
        /// <param name="Handle">Window to be restored</param>
        static public void RestoreWindow(IntPtr Handle)
        {
            ShowWindow(Handle, 9);
        }

        /// <summary>
        /// Changes the title of the window
        /// </summary>
        /// <param name="Handle">Affected window</param>
        /// <param name="Title">New title</param>
        static public void Rename(IntPtr Handle, string Title)
        {
            SetWindowTextA((int)Handle, Title);
        }



        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;

        /// <summary>
        /// Sets the given image as a desktop wallpaper. It must be in BMP format.
        /// </summary>
        /// <param name="FileName">Path to a BMP image</param>
        static public void SetDesktopWallpaper(string FileName)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, FileName, SPIF_UPDATEINIFILE);
        }

        /// <summary>
        /// Finds a window with the given title
        /// </summary>
        /// <param name="Title">Title to be searched</param>
        static public IntPtr Find(string Title)
        {
            return FindWindow(null, Title);
        }


        /// <summary>
        /// Moves a window to a given location
        /// </summary>
        /// <param name="Handle">Window to be moved</param>
        /// <param name="Position">Position where the window will get moved</param>
        static public void Move(IntPtr Handle, Point Position)
        {
            RECT rct = new RECT();
            GetWindowRect(Handle, ref rct);
            MoveWindow(Handle, Position.X, Position.Y, rct.right - rct.left, rct.bottom - rct.top, true);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);


        public static IntPtr GetWindowHandle(Process Proc, string Class, string WindowTitle)
        {
            return Windows.FindWindowEx(Proc.MainWindowHandle, IntPtr.Zero, Class, WindowTitle);
        }
        static public void MoveBorderless(IntPtr Handle)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }


    }
}
