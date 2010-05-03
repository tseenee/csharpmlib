using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace MLib.Imaging
{
    /// <summary>
    /// Functions for image manipulation
    /// </summary>
    public static class ImageManipulation
    {
        #region Funkcije
        public static Color GetColor(int X, int Y, byte[] colors, int width)
        {
            try
            {
                int sirina = width * 4;

                int poz = X * 4;
                poz += (Y * sirina);

                return Color.FromArgb(255, colors[poz + 2], colors[poz + 1], colors[poz]);
            }
            catch { }
            return Color.FromArgb(255, 255, 255, 255);
        }



        public static void SetColor(int X, int Y, Color color, byte[] colors, int width)
        {
            try
            {
                int sirina = width * 4;

                int poz = X * 4;
                poz += (Y * sirina);

                colors[poz] = color.B;
                colors[poz + 1] = color.G;
                colors[poz + 2] = color.R;
            }
            catch { }
        }
        #endregion

        #region GetBytes
        /// <summary>
        /// Returns image as bytes
        /// </summary>
        /// <param name="Bmp">Bitmap to be converted</param>
        /// <returns></returns>
        public static byte[] GetBytes(Bitmap Bmp)
        {
            try
            {
                Bitmap bmp = Bmp;
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
                IntPtr ptr = data.Scan0;
                int numBytes = data.Stride * bmp.Height;
                byte[] rgbValues = new byte[numBytes];
                Marshal.Copy(ptr, rgbValues, 0, numBytes);
                bmp.UnlockBits(data);

                return rgbValues;
            }
            catch {throw new Exception("There was an error while converting the image.");  }
        }
        #endregion


        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
        }

        /// <summary>
        /// Returns the image codec with the given mime type
        /// </summary>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        } 



        public static Bitmap SetColor(Bitmap TempImage, int Amount, byte ByteNumber)
        {
            Bitmap bmp = TempImage;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = data.Scan0;

            int numBytes = data.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];

            List<Point> Najdeno = new List<Point>();

            Marshal.Copy(ptr, rgbValues, 0, numBytes);


            for (int y = 0; y < rgbValues.Length; y += 4)
            {
                if (Amount > 255)
                    rgbValues[y + ByteNumber] = 255;
                else if (Amount < 0)
                    rgbValues[y + ByteNumber] = 0;
                else
                    rgbValues[y + ByteNumber] = (byte)Amount;

            }



            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(data);


            return TempImage;
        }

        public static Bitmap Povdari(Bitmap TempImage, int Amount , byte ByteNumber)
        {
            Bitmap bmp = TempImage;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = data.Scan0;

            int numBytes = data.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];

            List<Point> Najdeno = new List<Point>();

            Marshal.Copy(ptr, rgbValues, 0, numBytes);


            for (int y = 0; y < rgbValues.Length; y += 4)
            {
                if (rgbValues[y + ByteNumber] + Amount > 255)
                    rgbValues[y + ByteNumber] = 255;
                else
                    rgbValues[y + ByteNumber] += (byte)Amount;

            }



            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(data);


            return TempImage;
        }

        public static Bitmap Odstrani(Bitmap TempImage, int Amount, byte ByteNumber)
        {
            Bitmap bmp = TempImage;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = data.Scan0;

            int numBytes = data.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];

            List<Point> Najdeno = new List<Point>();

            Marshal.Copy(ptr, rgbValues, 0, numBytes);


            for (int y = 0; y < rgbValues.Length; y += 4)
            {
                if (rgbValues[y + ByteNumber] - Amount < 0)
                    rgbValues[y + ByteNumber] = 0;
                else
                    rgbValues[y + ByteNumber] -= (byte)Amount;

            }



            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(data);


            return TempImage;
        }


        public class ConvMatrix
        {
            public int TopLeft = 0, TopMid = 0, TopRight = 0;
            public int MidLeft = 0, Pixel = 1, MidRight = 0;
            public int BottomLeft = 0, BottomMid = 0, BottomRight = 0;
            public int Factor = 1;
            public int Offset = 0;
            public void SetAll(int nVal)
            {
                TopLeft = TopMid = TopRight = MidLeft = Pixel = MidRight = BottomLeft = BottomMid = BottomRight = nVal;
            }
        }



        public static Bitmap Invert(Bitmap b)
        {
            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                for (int y = 0; y < b.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        p[0] = (byte)(255 - p[0]);
                        ++p;
                    }
                    p += nOffset;
                }
            }

            b.UnlockBits(bmData);

            return b;
        }

        public static Bitmap Conv3x3(Bitmap b, ConvMatrix m)
        {
            // Avoid divide by zero errors
            if (0 == m.Factor) return (Bitmap)b.Clone();

            Bitmap bSrc = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            int stride2 = stride * 2;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = stride + 6 - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;

                int nPixel;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nPixel = ((((pSrc[2] * m.TopLeft) + (pSrc[5] * m.TopMid) + (pSrc[8] * m.TopRight) +
                            (pSrc[2 + stride] * m.MidLeft) + (pSrc[5 + stride] * m.Pixel) + (pSrc[8 + stride] * m.MidRight) +
                            (pSrc[2 + stride2] * m.BottomLeft) + (pSrc[5 + stride2] * m.BottomMid) + (pSrc[8 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[5 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[1] * m.TopLeft) + (pSrc[4] * m.TopMid) + (pSrc[7] * m.TopRight) +
                            (pSrc[1 + stride] * m.MidLeft) + (pSrc[4 + stride] * m.Pixel) + (pSrc[7 + stride] * m.MidRight) +
                            (pSrc[1 + stride2] * m.BottomLeft) + (pSrc[4 + stride2] * m.BottomMid) + (pSrc[7 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pSrc[0] * m.TopLeft) + (pSrc[3] * m.TopMid) + (pSrc[6] * m.TopRight) +
                            (pSrc[0 + stride] * m.MidLeft) + (pSrc[3 + stride] * m.Pixel) + (pSrc[6 + stride] * m.MidRight) +
                            (pSrc[0 + stride2] * m.BottomLeft) + (pSrc[3 + stride2] * m.BottomMid) + (pSrc[6 + stride2] * m.BottomRight)) / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;

                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pSrc += 3;
                    }

                    p += nOffset;
                    pSrc += nOffset;
                }
            }

            b.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);

            return b;
        }

        public static Bitmap GaussianBlur(Bitmap b, int nWeight /* default to 4*/)
        {

            ConvMatrix m = new ConvMatrix();
            m.SetAll(1);
            m.Pixel = nWeight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 2;
            m.Factor = nWeight + 12;
            Conv3x3(b, m);
            return b;
        }

        public static Bitmap Sharpen(Bitmap b, int nWeight /* default to 11*/ )
        {
            ConvMatrix m = new ConvMatrix();
            m.SetAll(0);
            m.Pixel = nWeight;
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = -2;
            m.Factor = nWeight - 8;
            Conv3x3(b, m);
            return b;
        }

        public static Bitmap EdgeDetect(Bitmap b)
        {
            ConvMatrix m = new ConvMatrix();
            m.TopLeft = m.TopMid = m.TopRight = -1;
            m.MidLeft = m.Pixel = m.MidRight = 0;
            m.BottomLeft = m.BottomMid = m.BottomRight = 1;

            m.Offset = 127;
            Conv3x3(b, m);
            return b;
        }

        #region MakeIcon
        /// <summary>
        /// Creates an icon from the given image
        /// </summary>
        /// <param name="Img">Image to be converted</param>
        /// <param name="Size">Icon size</param>
        /// <returns></returns>
        public static Icon MakeIcon(Image Img, int Size)
        {

            Bitmap square = new Bitmap(Size, Size);

            Graphics g = Graphics.FromImage(square);

            if ((Size != 48) && (Size != 128) && (Size != 256))
                throw new Exception("Unsupported size.");


            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(Img, 0, 0, 128, 128); // draw image with specified dimensions
            g.Flush(); // make sure all drawing operations complete before we get the icon
            return Icon.FromHandle(square.GetHicon());
        }
        #endregion

        #region Darken
        /// <summary>
        /// Darkens the image
        /// </summary>
        /// <param name="TempImage">Image to be darkened</param>
        /// <param name="Amount">Amount of darkness added</param>
        /// <returns>Darkened image</returns>
        public static Bitmap Darken(Bitmap TempImage, int Amount)
        {
            Bitmap bmp = TempImage;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = data.Scan0;

            int numBytes = data.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];

            List<Point> Najdeno = new List<Point>();

            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            for (int y = 0; y < rgbValues.Length; y += 4)
            {
                if (rgbValues[y + 0] - Amount < 0)
                    rgbValues[y + 0] = 0;
                else
                    rgbValues[y + 0] -= (byte)Amount;

                if (rgbValues[y + 1] - Amount < 0)
                    rgbValues[y + 1] = 0;
                else
                    rgbValues[y + 1] -= (byte)Amount;

                if (rgbValues[y + 2] - Amount < 0)
                    rgbValues[y + 2] = 0;
                else
                    rgbValues[y + 2] -= (byte)Amount;

            }


            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(data);


            return TempImage;
        }
        #endregion

        #region Brighten
        /// <summary>
        /// Brightens the image
        /// </summary>
        /// <param name="TempImage">Image to be brightened</param>
        /// <param name="Amount">Amount of brightness added</param>
        /// <returns>Brightened image</returns>
        public static Bitmap Brighten(Bitmap TempImage, int Amount)
        {
            Bitmap bmp = TempImage;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = data.Scan0;

            int numBytes = data.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];


            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            for (int y = 0; y < rgbValues.Length; y += 4)
            {
                if (rgbValues[y + 0] + Amount > 255)
                    rgbValues[y + 0] = 255;
                else
                    rgbValues[y + 0] += (byte)Amount;

                if (rgbValues[y + 1] + Amount > 255)
                    rgbValues[y + 1] = 255;
                else
                    rgbValues[y + 1] += (byte)Amount;

                if (rgbValues[y + 2] + Amount > 255)
                    rgbValues[y + 2] = 255;
                else
                    rgbValues[y + 2] += (byte)Amount;
            }
            

            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(data);


            return TempImage;
        }
        #endregion

        #region Poster Effect
        /// <summary>
        /// Applies poster effect on the image
        /// </summary>
        /// <param name="TempImage">Image to be modified</param>
        /// <returns>Modified image</returns>
        public static Bitmap PosterEffect(Bitmap TempImage)
        {
            Bitmap bmp = TempImage;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = data.Scan0;

            int numBytes = data.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

                for (int y = 0; y < rgbValues.Length; y += 7)
                {
                    rgbValues[y] = 255;
                }


            Marshal.Copy(rgbValues, 0, ptr, numBytes);
            bmp.UnlockBits(data);

            return bmp;
        }
        #endregion

        #region Pixelate
        /// <summary>
        /// Pixelates the image
        /// </summary>
        /// <param name="TempImage">Image to be pixelated</param>
        /// <param name="PixelSize">Size of the pixels</param>
        /// <returns>Pixelated image</returns>
        public static Bitmap Pixelate(Bitmap TempImage, int PixelSize)
        {
            Bitmap bmp = TempImage;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            IntPtr ptr = data.Scan0;

            int numBytes = data.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];

            List<Point> Najdeno = new List<Point>();

            Marshal.Copy(ptr, rgbValues, 0, numBytes);


            int b = PixelSize;




            for (int x = 0; x < bmp.Width; x += b)
                for (int y = 0; y < bmp.Height; y += b)
                {
                    int povpR = 0;
                    int povpG = 0;
                    int povpB = 0;

                    int pikslov = 0;

                    for(int i = 0; i < b; i++)
                        for (int n = 0; n < b; n++)
                        {
                            if ((x + i < bmp.Width) && ((y + n < bmp.Height)))
                            {
                                Color c = GetColor(x + i, y + n, rgbValues, bmp.Width);
                                povpR += c.R;
                                povpG += c.G;
                                povpB += c.B;
                                pikslov++;
                            }
                            else
                                break;
                        }

                    povpR = povpR / pikslov;
                    povpG = povpG / pikslov;
                    povpB = povpB / pikslov;


                    Color barva = Color.FromArgb(0, povpR, povpG, povpB);
                    for (int i = 0; i < b; i++)
                        for (int n = 0; n < b; n++)
                        {
                            if ((x + i < bmp.Width) && ((y + n < bmp.Height)))
                            {
                                SetColor(x + i, y + n, barva, rgbValues, bmp.Width);
                            }
                            else
                                break;
                        }
                }

                Marshal.Copy(rgbValues, 0, ptr, numBytes);
                bmp.UnlockBits(data);


            return TempImage;
        }
        #endregion

        #region ToBlackAndWhite
        /// <summary>
        /// Converts an image to black and white image
        /// </summary>
        /// <param name="TempImage">Image to be converted</param>
        /// <returns>Black and white image</returns>
        public static Bitmap ToBlackAndWhite(Bitmap TempImage)
        {
            ImageFormat ImageFormat = TempImage.RawFormat;
            Bitmap TempBitmap = new Bitmap(TempImage, TempImage.Width, TempImage.Height);

            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);
            Graphics NewGraphics = Graphics.FromImage(NewBitmap);
            float[][] FloatColorMatrix ={
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColorMatrix);

            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width,
                TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height,
                GraphicsUnit.Pixel, Attributes);

            NewGraphics.Dispose();
            return NewBitmap;
        }

        #endregion

        #region FindOnImage
        /// <summary>
        /// Finds an image in the given image
        /// </summary>
        /// <param name="StartX">X point to start looking</param>
        /// <param name="StartY">Y point to start looking</param>
        /// <param name="EndX">X point to stop looking</param>
        /// <param name="EndY">Y point to stop looking</param>
        /// <param name="ImateToSearchOn">The image it will search in</param>
        /// <param name="ImageToFind">Image it will search for</param>
        /// <param name="MistakesAllowed">How many mistakes are allowed</param>
        /// <returns></returns>
        public static Point FindOnImage(int StartX, int StartY, int EndX, int EndY, Bitmap ImateToSearchOn, Bitmap ImageToFind, int MistakesAllowed)
        {
            for (int i = StartX; i < EndX; i++)
                for (int i2 = StartY; i2 < EndY; i2++)
                {
                    int Mistakes = 0;
                    bool DoBreak = false;
                    bool Found = false;
                    for (int x = 0; ((x < ImageToFind.Width) && (!DoBreak)); x++)
                        for (int y = 0; ((y < ImageToFind.Height) && (!DoBreak)); y++)
                        {
                            if (ImateToSearchOn.GetPixel(i + x, i2 + y).ToArgb() == ImageToFind.GetPixel(x, y).ToArgb())
                            {
                                Found = true;
                            }
                            else if (MistakesAllowed >= Mistakes)
                            {
                                Mistakes++;
                            }
                            else
                            {
                                DoBreak = true;
                                Found = false;
                                break;
                            }

                        }

                    if (Found)
                    {
                        return new Point(i, i2);
                    }
                }

            return new Point(-1, -1);
        }

        #endregion
    }
}
