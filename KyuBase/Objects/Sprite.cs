using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Compression;
using System.Text;

namespace KyuBase.Objects
{
    public class Sprite
    {
        public int x, y;
        public int h, w;

        public Bitmap image;
        public Graphics imgGraphics;

        public string name;

        /// <summary>
        /// This only needs to be declared when we are exporting/saving maps.
        /// </summary>
        public String filepath;

        /// <summary>
        /// Create a new sprite instance
        /// </summary>
        /// <param name="x">x-axis</param>
        /// <param name="y">y-axis</param>
        /// <param name="h">height</param>
        /// <param name="w">width</param>
        public Sprite(int x, int y, int h, int w, Bitmap img)
        {
            this.x = x;
            this.y = y;
            this.h = h;
            this.w = w;
            if (img != null)
            {
                Bitmap bdi = img;
                Bitmap lks = new Bitmap(w, h);
                Graphics.FromImage(lks).DrawImage(bdi, 0, 0);
                image = lks;
                bdi.Dispose();
            }
        }

        public Sprite(Bitmap img)
        {
            if (img != null)
            {
                Bitmap bdi = img;
                Bitmap lks = new Bitmap(img.Width, img.Height);
                Graphics.FromImage(lks).DrawImage(bdi, 0, 0);
                image = lks;
                bdi.Dispose();
            }
        }

        public Sprite(string location, ref ZipArchive img)
        {
            throw new NotImplementedException();
        }

        public Sprite(String arr)
        {
            arr = arr.Substring(1, arr.Length - 2);
            //string a[] = arr.Split(',');
            string[] a = arr.Split(',');
            x = int.Parse(a[0]);
            y = int.Parse(a[1]);
            h = int.Parse(a[2]);
            w = int.Parse(a[3]);
            filepath = a[4];
            image = (Bitmap)Bitmap.FromFile(filepath);
        }

        public Graphics GetGraphics()
        {
            switch (imgGraphics == null)
            {
                case false:
                    return imgGraphics;
                case true:
                    imgGraphics = Graphics.FromImage(image);
                    return imgGraphics;
            }
        }

        ~Sprite()
        {
            x = 0;
            y = 0;
            h = 0;
            w = 0;
            try
            {
                //image.Dispose();
            }
            catch
            {

            }
            try
            {
                //imgGraphics.Dispose();
            }
            catch
            {

            }
        }

        private void UpdateBitmap(float rotation)
        {
            Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
            rotatedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(image.Width / 2, image.Height / 2);
                g.RotateTransform(rotation);
                g.TranslateTransform(-image.Width / 2, -image.Height / 2);
                g.DrawImage(image, new Point(0, 0));
            }

            image = rotatedImage;
        }

        public Sprite newSprite() => new Sprite(x, y, h, w, (Bitmap)image.Clone());
    }
}
