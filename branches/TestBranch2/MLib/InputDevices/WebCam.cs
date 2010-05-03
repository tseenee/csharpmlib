using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace MLib.InputDevices
{
    /// <summary>
    /// WebCam image capture system
    /// </summary>
    public class WebCam
    {

        public class WebCamEventArgs : EventArgs
        {
            private Image m_Image;
            private ulong m_FrameNumber = 0;

            public WebCamEventArgs()
            {
            }

            /// <summary>
            ///  WebCamImage
            ///  This is the image returned by the web camera capture
            /// </summary>
            public Image WebCamImage
            {
                get
                { return m_Image; }

                set
                { m_Image = value; }
            }

            /// <summary>
            /// FrameNumber
            /// Holds the sequence number of the frame capture
            /// </summary>
            public ulong FrameNumber
            {
                get
                { return m_FrameNumber; }

                set
                { m_FrameNumber = value; }
            }
        }



        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
        public WebCam()
        {
            timer1.Tick += new EventHandler(timer1_Tick);
        }


        // property variables
        private int m_TimeToCapture_milliseconds = 100;
        private int m_Width = 640;
        private int m_Height = 480;
        private int mCapHwnd;
        private ulong m_FrameNumber = 0;

        // global variables to make the video capture go faster
        private WebCamEventArgs x = new WebCamEventArgs();
        private IDataObject tempObj;
        private System.Drawing.Image tempImg;
        private bool bStopped = true;

        // event delegate
        public delegate void WebCamEventHandler(object source, WebCamEventArgs e);
        // fired when a new image is captured
        public event WebCamEventHandler ImageCaptured;

        #region API Declarations

        [DllImport("user32", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        [DllImport("avicap32.dll", EntryPoint = "capCreateCaptureWindowA")]
        private static extern int capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int X, int Y, int nWidth, int nHeight, int hwndParent, int nID);

        [DllImport("user32", EntryPoint = "OpenClipboard")]
        private static extern int OpenClipboard(int hWnd);

        [DllImport("user32", EntryPoint = "EmptyClipboard")]
        private static extern int EmptyClipboard();

        [DllImport("user32", EntryPoint = "CloseClipboard")]
        private static extern int CloseClipboard();

        #endregion

        #region API Constants

        private const int WM_USER = 1024;

        private const int WM_CAP_CONNECT = 1034;
        private const int WM_CAP_DISCONNECT = 1035;
        private const int WM_CAP_GET_FRAME = 1084;
        private const int WM_CAP_COPY = 1054;

        private const int WM_CAP_START = WM_USER;

        private const int WM_CAP_DLG_VIDEOFORMAT = WM_CAP_START + 41;
        private const int WM_CAP_DLG_VIDEOSOURCE = WM_CAP_START + 42;
        private const int WM_CAP_DLG_VIDEODISPLAY = WM_CAP_START + 43;
        private const int WM_CAP_GET_VIDEOFORMAT = WM_CAP_START + 44;
        private const int WM_CAP_SET_VIDEOFORMAT = WM_CAP_START + 45;
        private const int WM_CAP_DLG_VIDEOCOMPRESSION = WM_CAP_START + 46;
        private const int WM_CAP_SET_PREVIEW = WM_CAP_START + 50;

        #endregion

        #region NOTES
        #endregion

        Bitmap c_Image = new Bitmap(640,480,0, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, IntPtr.Zero);

        #region Properties
        ~WebCam()
        {
            Stop();
        }

        public int Interval
        {
            get
            { return m_TimeToCapture_milliseconds; }

            set
            { m_TimeToCapture_milliseconds = value; }
        }


        public int Height
        {
            get
            { return m_Height; }

            set
            { m_Height = value; }
        }

        public Bitmap CurrentImage
        {
            get
            { 
                return c_Image; 
            }
        }

        public int Width
        {
            get
            { return m_Width; }

            set
            { m_Width = value; }
        }

        public ulong FrameNumber
        {
            get
            { return m_FrameNumber; }

            set
            { m_FrameNumber = value; }
        }
        #endregion

        //ulong
        public void Start(int Interval)
        {
            try
            {

                Stop();
                mCapHwnd = capCreateCaptureWindowA("WebCap", 0, 0, 0, m_Width, m_Height, 0, 0);
                Application.DoEvents();
                SendMessage(mCapHwnd, WM_CAP_CONNECT, 0, 0);
                SendMessage(mCapHwnd, WM_CAP_SET_PREVIEW, 0, 0);

                //m_FrameNumber = FrameNum;
                bStopped = false;
                timer1.Interval = (int)Interval;
                timer1.Start();
            }
            catch
            {
                Stop();
            }
        }

        public void Stop()
        {
            try
            {
                bStopped = true;

                Application.DoEvents();
                SendMessage(mCapHwnd, WM_CAP_DISCONNECT, 0, 0);
            }
            catch
            {
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // pause the timer
                timer1.Stop();
                object tmp = Clipboard.GetDataObject();
                // get the next frame;
                SendMessage(mCapHwnd, WM_CAP_GET_FRAME, 0, 0);

                // copy the frame to the clipboard
                SendMessage(mCapHwnd, WM_CAP_COPY, 0, 0);

                // paste the frame into the event args image
                if (ImageCaptured != null)
                {
                    // get from the clipboard
                    tempObj = Clipboard.GetDataObject();
                    tempImg = (System.Drawing.Bitmap)tempObj.GetData(System.Windows.Forms.DataFormats.Bitmap);
                    //Clipboard.SetDataObject(tmp);
                    /*
                    * For some reason, the API is not resizing the video
                    * feed to the width and height provided when the video
                    * feed was started, so we must resize the image here
                    */
                    //Clipboard.SetDataObject(tmp);
                    x.WebCamImage = tempImg.GetThumbnailImage(m_Width, m_Height, null, System.IntPtr.Zero);
                    c_Image = (Bitmap)x.WebCamImage;
                    // raise the event
                    this.ImageCaptured(this, x);
                }

                // restart the timer
                Application.DoEvents();
                if (!bStopped)
                    timer1.Start();
            }

            catch
            {
                Stop();
            }
        }
    }
}
