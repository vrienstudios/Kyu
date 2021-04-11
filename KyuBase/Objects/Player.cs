using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace KyuBase.Objects
{
    public class Player
    {
        public int x, y;
        public Sprite Sprite;

        private float _rotation;

        /// <summary>
        /// Rotate player with float value 0-360
        /// </summary>
        public float rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                UpdateBitmap(value);
            }
        }

        /// <summary>
        /// This is a playor object based off of sprite.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="sprite"></param>
        public Player(int x, int y, Sprite sprite)
        {
            this.Sprite = sprite;
            this.x = x;
            this.y = y;
        }

        public Player(Sprite sprite)
        {
            this.Sprite = sprite;
        }

        private void UpdateBitmap(float rotation)
        {
            Bitmap rotatedImage = new Bitmap(this.Sprite.image.Width, this.Sprite.image.Height);
            rotatedImage.SetResolution(this.Sprite.image.HorizontalResolution, this.Sprite.image.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(this.Sprite.image.Width / 2, this.Sprite.image.Height / 2);
                g.RotateTransform(rotation);
                g.TranslateTransform(-this.Sprite.image.Width / 2, -this.Sprite.image.Height / 2);
                g.DrawImage(this.Sprite.image, new Point(0, 0));
            }

            this.Sprite.image = rotatedImage;
        }
    }
}
