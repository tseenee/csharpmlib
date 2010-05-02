using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MLib.Imaging
{
    /// <summary>
    /// Functions for working with desktop/monitors
    /// </summary>
    public static class Desktop
    {
        #region SetWallpaper
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 0x1;

        /// <summary>
        /// Sets a BMP image as a wallpaper
        /// </summary>
        /// <param name="filename">Must be a BMP image</param>
        public static void SetWallpaper(string FileName)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, FileName, SPIF_UPDATEINIFILE);
        }
        #endregion

        #region Screen Size
        private static int ScreenWidth()
        {
            int X = 0;
            int Y = Screen.PrimaryScreen.Bounds.Height;

            foreach (Screen scrn in Screen.AllScreens)
                X += scrn.Bounds.Width;

            return X;
        }

        public static Size ScreenSize
        {
            get
            {
                return new Size(ScreenWidth(), Screen.PrimaryScreen.Bounds.Height);
            }
        }
        #endregion

        #region GetScreenPixel
        /// <summary>
        /// Gets the color from the screen at given coordinates
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <returns></returns>
        public static Color GetScreenPixel(int X, int Y)
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            Graphics grp = Graphics.FromImage(bmp);
            grp.CopyFromScreen(new Point(X, Y), Point.Empty, new Size(1, 1));
            grp.Save();


            return bmp.GetPixel(0, 0);
        }
        #endregion


        #region SlowFunctions
        public static Color GetScreenPixelSlow(int X, int Y)
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            Graphics grp = Graphics.FromImage(bmp);
            grp.CopyFromScreen(Point.Empty, Point.Empty, new Size(X, Y));
            grp.Save();


            return bmp.GetPixel(bmp.Width - 1, bmp.Height - 1);
        }

        /// <summary>
        /// Makes a screenshot
        /// </summary>
        /// <param name="Start">Start point of the desktop</param>
        /// <param name="End">End point of the desktop</param>
        /// <returns>Bitmap of the screen</returns>
        public static Bitmap ScreenshotSlow(Point Start, Point End)
        {
            int X = End.X - Start.X;
            int Y = End.Y - Start.Y;
            Bitmap bmp = new Bitmap(X, Y, PixelFormat.Format32bppPArgb);
            Graphics grp = Graphics.FromImage(bmp);
            int sx = End.X - Start.X;
            int sy = End.Y - Start.Y;
            grp.CopyFromScreen(Point.Empty, Point.Empty, new Size(sx, sy));
            grp.Save();
            int x = End.X - Start.X;
            int y = End.Y - Start.Y;
            Bitmap slika = new Bitmap(x, y);
            Graphics gr = Graphics.FromImage(slika);
            
            gr.DrawImage(bmp, new Point(-Start.X, -Start.Y));
            return slika;
        }
        #endregion


        #region Screenshot
        /// <summary>
        /// Makes a screenshot
        /// </summary>
        /// <returns>Bitmap of the screen</returns>
        public static Bitmap Screenshot()
        {
            int X = 0;
            int Y = Screen.PrimaryScreen.Bounds.Height;

            foreach (Screen scrn in Screen.AllScreens)
                X += scrn.Bounds.Width;

            Bitmap bmp = new Bitmap(X, Y, PixelFormat.Format32bppPArgb);
            Graphics grp = Graphics.FromImage(bmp);
            grp.CopyFromScreen(Point.Empty, Point.Empty, new Size(X, Y));
            grp.Save();
            return bmp;
        }

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, uint nFlags);



        /// <summary>
        /// Makes a screenshot
        /// </summary>
        /// <param name="Start">Start point of the desktop</param>
        /// <param name="End">End point of the desktop</param>
        /// <returns>Bitmap of the screen</returns>
        public static Bitmap Screenshot(Point Start, Point End)
        {
            int X = End.X - Start.X;
            int Y = End.Y - Start.Y;
            Bitmap bmp = new Bitmap(X, Y, PixelFormat.Format32bppPArgb);
            Graphics grp = Graphics.FromImage(bmp);
            int sx = End.X - Start.X;
            int sy = End.Y - Start.Y;
            grp.CopyFromScreen(Start, Point.Empty, new Size(sx, sy));
            grp.Save();
            return bmp;
        }
        #endregion

    }
}
