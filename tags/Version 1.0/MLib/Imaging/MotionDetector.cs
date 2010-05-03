using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MLib.Imaging
{
    public class MotionDetector
    {
        class Objekt
        {
            public Rectangle Rect;
            public int Jakost = 0;
            public Grupa Group = null;
        }

        class Grupa
        {
            public List<Objekt> Objekti = new List<Objekt>();
        }

        void Ujemanje(Objekt o1)
        {
            //Recursively groups all the objects and neighbour objects
            for (int i = 0; i < Objekti.Count; i++)
            {
                try
                {
                    if ((Objekti[i] != o1) && (Objekti[i].Group != o1.Group) && (Objekti[i].Rect.IntersectsWith(o1.Rect)))
                    {
                        Objekti[i].Group = o1.Group;
                        o1.Group.Objekti.Add(Objekti[i]);
                        Ujemanje(Objekti[i]);
                    }
                }
                catch { }
            }
        }

        #region Funkcije
        private static byte GetColor(int X, int Y, byte[] colors, int width)
        {
            int sirina = width * 4;

            int poz = X * 4;
            poz += (Y * sirina);

            return (byte)((colors[poz + 2] + colors[poz + 1] + colors[poz]) / 3);
        }

        private static void SetColor(int X, int Y, Color color, byte[] colors, int width)
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

        #region DetectMotion

        /// <summary>
        /// Calculates motion between two images
        /// </summary>
        /// <param name="FirstImage">Firs image</param>
        /// <param name="SecondImage">Second image</param>
        /// <param name="Sensitivity">Minimum color difference</param>
        /// <param name="Preciseness">Pixel size</param>
        /// <returns>Motion detected</returns>
        public bool DetectMotion(Bitmap FirstImage, Bitmap SecondImage, int Sensitivity, int Preciseness, bool Pixelize)
        {

            if (FirstImage.Size != SecondImage.Size)
                throw new Exception("Image sizes must be the same.");


            int l = Sensitivity;

                try
                {
                    Bitmap original = (Bitmap)((Bitmap)SecondImage).GetThumbnailImage(SecondImage.Width, SecondImage.Height, null, new IntPtr());
                    FirstImage = ImageManipulation.ToBlackAndWhite(FirstImage);
                    SecondImage = ImageManipulation.ToBlackAndWhite(SecondImage);



                    int b = Preciseness;
                    if (Pixelize)
                    {
                        ImageManipulation.Pixelate(FirstImage, b);
                        ImageManipulation.Pixelate(SecondImage, b);
                    }


                    byte[] rgb1 = ImageManipulation.GetBytes(FirstImage);
                    byte[] rgb2 = ImageManipulation.GetBytes(SecondImage);



                    Graphics grp = Graphics.FromImage(original);
                    Objekti.Clear();
                    Grupe.Clear();

                    //Copy Start

                    for (int y = 0; y < FirstImage.Height - 4; y += b)
                        for (int x = 0; x < FirstImage.Width - 4; x += b)
                        {
                            byte barva1 = GetColor(x, y, rgb1, FirstImage.Width);
                            byte barva2 = GetColor(x, y, rgb2, SecondImage.Width);
                            int raz = Math.Abs(barva1 - barva2);
                            if (raz > l)
                            {
                                Objekt obj = new Objekt();
                                obj.Rect = new Rectangle(x - b, y - b, b + b, b + b);
                                obj.Jakost = raz;
                                Objekti.Add(obj);

                            }

                        }



                    for (int i = 0; i < Objekti.Count; i++)
                    {
                        if (Objekti[i].Group == null)
                        {
                            Grupa grupa = new Grupa();

                            Objekt o1 = Objekti[i];
                            grupa.Objekti.Add(o1);
                            o1.Group = grupa;

                            Ujemanje(o1);

                            Grupe.Add(grupa);
                        }

                    }

                    Najdeno.Clear();
                    foreach (Grupa grupa in Grupe)
                    {
                        int miX = grupa.Objekti[0].Rect.X;
                        int miY = grupa.Objekti[0].Rect.Y;

                        int maX = grupa.Objekti[0].Rect.X;
                        int maY = grupa.Objekti[0].Rect.Y;

                        foreach (Objekt o in grupa.Objekti)
                        {
                            if (o.Rect.X < miX)
                                miX = o.Rect.X;
                            if (o.Rect.X + o.Rect.Width > maX)
                                maX = o.Rect.X + o.Rect.Width;

                            if (o.Rect.Y < miY)
                                miY = o.Rect.Y;
                            if (o.Rect.Y + o.Rect.Height > maY)
                                maY = o.Rect.Y + o.Rect.Height;
                        }
                        Najdeno.Add(new Rectangle(miX, miY, maX - miX, maY - miY));

                    }

                    for (int i = 0; i < Grupe.Count; i++)
                    {
                        foreach (Objekt obj in Grupe[i].Objekti)
                        {
                            if (i == 0)
                                grp.DrawRectangle(Pens.Blue, obj.Rect);
                            //grp.DrawRectangle(new Pen(Color.FromArgb(obj.Jakost, 0, 0)), obj.Rect);
                            else if (i == 1)
                                grp.DrawRectangle(Pens.Red, obj.Rect);
                            else if (i == 2)
                                grp.DrawRectangle(Pens.Green, obj.Rect);
                            else if (i == 3)
                                grp.DrawRectangle(Pens.Pink, obj.Rect);
                            else if (i == 4)
                                grp.DrawRectangle(Pens.Blue, obj.Rect);
                            else if (i == 5)
                                grp.DrawRectangle(Pens.Orange, obj.Rect);
                            else if (i == 6)
                                grp.DrawRectangle(Pens.Orchid, obj.Rect);
                            else if (i == 7)
                                grp.DrawRectangle(Pens.Snow, obj.Rect);
                            else if (i == 8)
                                grp.DrawRectangle(Pens.LightYellow, obj.Rect);
                            else if (i == 9)
                                grp.DrawRectangle(Pens.LightYellow, obj.Rect);
                            else if (i == 10)
                                grp.DrawRectangle(Pens.Gainsboro, obj.Rect);
                            else if (i == 11)
                                grp.DrawRectangle(Pens.Purple, obj.Rect);
                            else if (i == 12)
                                grp.DrawRectangle(Pens.HotPink, obj.Rect);
                            else
                                grp.DrawRectangle(Pens.White, obj.Rect);

                        }
                    }

                    //for (int i = 0; i < Najdeno.Count; i++)
                    //    grp.DrawRectangle(Pens.Yellow, Najdeno[i]);

                    //double Sens = 10;
                    for (int i = 0; i < Najdeno.Count; i++)
                    {
                        grp.DrawRectangle(Pens.Yellow, Najdeno[i]);
                    }


                    MotionRectangles.Clear();
                    MotionRectangles.AddRange(Najdeno);

                    //Copy End
                    grp.Save();

                    ProcessedImage = original;


                    if (MotionRectangles.Count > 0)
                        return true;
                    else
                        return false;

                }
                catch { throw new Exception("There was an error while trying to calculate motion detection."); }


        }
        #endregion


        public Bitmap ProcessedImage = null;
        List<Grupa> Grupe = new List<Grupa>();
        List<Objekt> Objekti = new List<Objekt>();
        List<Rectangle> Najdeno = new List<Rectangle>();
        public List<Rectangle> MotionRectangles = new List<Rectangle>();
    }
}
