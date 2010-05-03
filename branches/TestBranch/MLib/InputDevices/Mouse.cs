using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
namespace MLib.InputDevices
{
    /// <summary>
    /// Compilation of Mouse functions
    /// </summary>
    public static class Mouse
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        private const int MOUSEEVENTF_MIDDLEDOWN = 0x020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x040;

        /// <summary>
        /// Simulates a mouse click
        /// </summary>
        /// <param name="Button">Mouse button to be simulated</param>
        public static void Click(MouseButtons Button)
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            if(Button == MouseButtons.Left)
                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            else if(Button == MouseButtons.Right)
                mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
            else if (Button == MouseButtons.Middle)
                mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, X, Y, 0, 0);
            else
                throw new ArgumentNullException("This type of argument is not yet supported."); 

        }

        /// <summary>
        /// Simulates mouse button hold
        /// </summary>
        /// <param name="Button">Mouse button to be held</param>
        public static void Hold(MouseButtons Button)
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            if (Button == MouseButtons.Left)
                mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
            else if (Button == MouseButtons.Right)
                mouse_event(MOUSEEVENTF_RIGHTDOWN, X, Y, 0, 0);
            else if (Button == MouseButtons.Middle)
                mouse_event(MOUSEEVENTF_MIDDLEDOWN, X, Y, 0, 0);
            else
                throw new ArgumentNullException("This type of argument is not yet supported.");

        }

        /// <summary>
        /// Simulates mouse button release
        /// </summary>
        /// <param name="Button">Mouse button to be released</param>
        public static void Release(MouseButtons Button)
        {
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            if (Button == MouseButtons.Left)
                mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
            else if (Button == MouseButtons.Right)
                mouse_event(MOUSEEVENTF_RIGHTUP, X, Y, 0, 0);
            else if (Button == MouseButtons.Middle)
                mouse_event(MOUSEEVENTF_MIDDLEUP, X, Y, 0, 0);
            else
                throw new ArgumentNullException("This type of argument is not yet supported."); 

        }

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_RBUTTONDBLCLK = 0x0206;
        private const int WM_MOUSEMOVE = 0x0200;

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern byte VkKeyScan(char ch);



        public static void SimulateInput(Process Proc, string Class, string WindowTitle, int X, int Y, MouseButtons mb)
        {
            IntPtr hWnd = FindWindowEx(Proc.MainWindowHandle, IntPtr.Zero, Class, WindowTitle);
            SimulateInput(Proc, hWnd, X, Y, mb);
        }

        public static void SimulateInput(Process Proc, IntPtr WindowHandle, int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Left)
            {
                PostMessage(WindowHandle, WM_LBUTTONDOWN, 0, new IntPtr(Y * 0x10000 + X));
                Thread.Sleep(25);
                PostMessage(WindowHandle, WM_LBUTTONUP, 0, new IntPtr(Y * 0x10000 + X));
            }
            else
            {
                PostMessage(WindowHandle, WM_RBUTTONDOWN, 0, new IntPtr(Y * 0x10000 + X));
                Thread.Sleep(25);
                PostMessage(WindowHandle, WM_RBUTTONUP, 0, new IntPtr(Y * 0x10000 + X));
            }
        }



        /// <summary>
        /// Gets or sets the current position of the mouse
        /// </summary>
        public static Point Position
        {
            get
            {
                return Cursor.Position;
            }
            set
            {
                Cursor.Position = value;
            }
        }
    }
}
