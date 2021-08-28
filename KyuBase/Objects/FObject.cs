using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KyuBase.Objects
{
    public enum Evnts
    {
        OnClick = 0,
        OnMouseHover = 1,
        OnMouseLeave = 2,
    }

    public class FObject
    {
        private int width;

        private Bitmap _window;

        // Main display
        public Bitmap window;
        public Font font; // https://docs.microsoft.com/en-us/dotnet/api/system.drawing.font.-ctor?view=dotnet-plat-ext-3.1
        public String text;

        //Buffers to be written to main display.
        private Bitmap bgBuffer;
        private Bitmap textBuffer;

        //All Graphics objects
        public Graphics graphics;
        private Graphics bgGraphics;
        public Graphics textGraphics;

        public delegate void writingFinshed(FObject fobj);
        // Only thrown when procedural drawing functions end.
        public event writingFinshed onWritingFinished;

        public delegate void mouseLeave(FObject fobj);
        public event mouseLeave onMouseLeave;

        private void triggerWritingFinished()
        {
            if (onWritingFinished != null)
                onWritingFinished(this);
        }


        public Color FontColor
        {
            get
            {
                return FontColor;
            }
            set
            {
                FontColor = value;
                fontColor.Color = value;
            }
        }

        private SolidBrush fontColor = new SolidBrush(Color.White); //init on creation | TODO: implement font change setting

        //The point at which this FObject exists.
        public int x, y;

        //The prioritization on clicks: higher means it will be on top of lower ones.
        public int hierachy = 2; // 2 = menu
        public int[,] pos;

        [Obsolete]
        private bool clickFlag
        {
            set
            {
                if (onClick != null)
                {
                    onClick(this, x, y);
                }
            }
        }

        /// <summary>
        /// Call an event from this FObject.
        /// </summary>
        /// <param name="evnt"></param>
        public void callEvnt(Evnts evnt, int x, int y)
        {
            switch (evnt)
            {
                case Evnts.OnClick:
                    if (onClick != null)
                        onClick(this, x, y);
                    break;
                case Evnts.OnMouseHover:
                    if (onMouseHover != null)
                        onMouseHover(this, x, y);
                    break;
                case Evnts.OnMouseLeave:
                    if (onMouseLeave != null)
                        onMouseLeave(this);
                    break;
            }
        }

        private double opacity = 1.0;

        public bool ChangeOpacity(double o)
        {
            this.window = ChangeImageOpacity(window, o);
            this.bgBuffer = ChangeImageOpacity(bgBuffer, o);
            this.textBuffer = ChangeImageOpacity(textBuffer, o);
            opacity = o;
            updateWindow();
            return true;
        }

        public Task AnimatedOpacityChange(double op)
        {
            if (op < opacity)
            {
                return Task.Run(() =>
                {
                    do
                    {
                        opacity = opacity - 0.01;
                    n:
                        try
                        {
                            this.window = ChangeImageOpacity(window, opacity);
                            this.bgBuffer = ChangeImageOpacity(bgBuffer, opacity);
                            this.textBuffer = ChangeImageOpacity(textBuffer, opacity);
                        }
                        catch
                        {
                            goto n;
                        }
                        Thread.Sleep(60);
                    }
                    while (op < opacity);
                });
            }
            else
            {
                return Task.Run(() =>
                {
                    do
                    {
                        opacity = opacity + 0.1;
                    n:
                        try
                        {
                            this.window = ChangeImageOpacity(window, opacity);
                            this.bgBuffer = ChangeImageOpacity(bgBuffer, opacity);
                            this.textBuffer = ChangeImageOpacity(textBuffer, opacity);
                        }
                        catch
                        {
                            goto n;
                        }
                        Thread.Sleep(60);
                    }
                    while (op > opacity);
                });
            }
        }

        private Bitmap ChangeImageOpacity(Bitmap img, double opacity)
        {
            if ((img.PixelFormat & PixelFormat.Indexed) == PixelFormat.Indexed)
                throw new Exception("Can not change opacity of indexed image.");

            Bitmap bmp = (Bitmap)img.Clone();
            PixelFormat pxf = PixelFormat.Format32bppArgb;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, pxf);
            IntPtr ptr = bmpData.Scan0;
            int numBytes = bmp.Width * bmp.Height * 4;
            byte[] argbValues = new byte[numBytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, argbValues, 0, numBytes);
            for (int counter = 0; counter < argbValues.Length; counter += 4)
            {
                if (argbValues[counter + 4 - 1] == 0)
                    continue;

                int pos = 0;
                pos++;
                pos++;
                pos++;

                argbValues[counter + pos] = (byte)(255 * opacity);
            }
            System.Runtime.InteropServices.Marshal.Copy(argbValues, 0, ptr, numBytes);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        /// <summary>
        /// Creates an FObject to be displayed at the specified coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="window"></param>
        public FObject(int x, int y, Bitmap window)
        {
            this.window = new Bitmap(window.Width, window.Height);
            this.window.MakeTransparent();
            this.bgBuffer = new Bitmap(window.Width, window.Height);
            this.textBuffer = new Bitmap(window.Width, window.Height);

            Graphics.FromImage(this.bgBuffer).DrawImage(window, new Rectangle() { X = 0, Y = 0, Height = window.Height, Width = window.Width });
            graphics = Graphics.FromImage(this.window);
            graphics.FillRectangle(new SolidBrush(Color.Transparent), 0, 0, window.Width, window.Height);
            bgGraphics = Graphics.FromImage(this.bgBuffer);
            textGraphics = Graphics.FromImage(this.textBuffer);
            textGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            textGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            this.x = x;
            this.y = y;
            this.font = new Font(FontFamily.GenericSerif, 12);
            this.width = window.Width - 20;
            updateWindow();
        }

        /// <summary>
        /// Creates an FObject at the specified coordinates with a specific font.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public FObject(int x, int y, Bitmap window, Font font)
        {
            this.window = new Bitmap(window.Width, window.Height);
            this.window.MakeTransparent();
            this.bgBuffer = (Bitmap)window.Clone();
            this.textBuffer = new Bitmap(window.Width, window.Height);

            graphics = Graphics.FromImage(this.window);
            bgGraphics = Graphics.FromImage(this.bgBuffer);
            textGraphics = Graphics.FromImage(this.textBuffer);
            textGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            textGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            this.x = x;
            this.y = y;
            this.font = font;
            this.width = window.Width - 20;
            updateWindow();
            //updateBitmaps(this.window);
        }

        //Draws both buffers to the main window.
        private void updateWindow()
        {
            graphics.Clear(Color.Transparent);
            graphics.DrawImage(bgBuffer, 0, 0);
            graphics.DrawImage(textBuffer, 0, 0);
            GC.Collect();
        }

        public void updateBitmaps(Bitmap mnb)
        {
            this.window = new Bitmap(mnb.Width, mnb.Height);
            this.window.MakeTransparent();
            this.bgBuffer = mnb;
            this.textBuffer = new Bitmap(mnb.Width, mnb.Height);

            graphics = Graphics.FromImage(this.window);
            bgGraphics = Graphics.FromImage(this.bgBuffer);
            textGraphics = Graphics.FromImage(this.textBuffer);
            textGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            textGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            updateWindow();
        }

        //Clears the text buffer
        public void ClearText()
        {
            textBuffer = new Bitmap(window.Width, window.Height);
            textGraphics = Graphics.FromImage(textBuffer);
            textGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            textGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            updateWindow();
        }

        //Events
        public delegate void Click(FObject fObject, int x, int y);
        public event Click onClick;

        public delegate void MouseMove(FObject fObject, int x, int y);
        public event MouseMove onMouseHover;

        //Draw a string at the x,y on the FObject.
        public void DrawString(int x, int y, string str)
        {
            this.text = str;
            try
            {
                textGraphics.DrawString(str, font, fontColor, x, y);
            }
            catch
            {
                if (font == null)
                    throw new Exception("font not set");
            }
            updateWindow();
        }

        public void RotateBitmap(float rotation)
        {
            Bitmap rotatedImage = new Bitmap(bgBuffer.Width, bgBuffer.Height);
            rotatedImage.SetResolution(window.HorizontalResolution, window.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(bgBuffer.Width / 2, bgBuffer.Height / 2);
                g.RotateTransform(rotation);
                g.TranslateTransform(-bgBuffer.Width / 2, -bgBuffer.Height / 2);
                g.DrawImage(bgBuffer, new Point(0, 0));
            }

            bgBuffer = rotatedImage;
            updateWindow();
        }

        //Draw a string in the center of the FObject
        public void DrawStringCenter(string str)
        {
            this.text = str;
            try
            {
                textGraphics.DrawString(str, font, fontColor, (this.window.Width / 2) - (graphics.MeasureString(str, font).Width / 2), (this.window.Height / 2) - (graphics.MeasureString(str, font).Height / 2));
            }
            catch
            {
                if (font == null)
                    throw new Exception("font not set");
            }
            updateWindow();
        }

        //Draw a string in the center of the FObject
        public void ProceduralDrawStringCenter(string str)
        {
            this.text = str;
            try
            {
                textGraphics.DrawString(str, font, fontColor, (this.window.Width / 2) - (graphics.MeasureString(str, font).Width / 2), (this.window.Height / 2) - (graphics.MeasureString(str, font).Height / 2));
            }
            catch
            {
                if (font == null)
                    throw new Exception("font not set");
            }
            updateWindow();
        }

        //Draw a string procedurally at a set interval to the FObject.
        public Task ProceduralDrawString(int x, int y, int interval, string str, int mx = -1, Action onFinished = null, bool autoAdjust = true)
        {
            this.text = str;
            string[] spl = str.Split(' ');
            return Task.Run(() =>
            {
                int id = x;
                switch (mx)
                {
                    case -1:
                        {
                            for (int i = 0; i < spl.Length; i++)
                            {
                                textGraphics.DrawString(spl[i].ToString(), font, fontColor, x, y);
                                x += (int)(textGraphics.MeasureString(spl[i].ToString(), font).Width - 4);
                                x += 4;
                                if (x >= width - 30)
                                {
                                    switch (autoAdjust)
                                    {
                                        case true:
                                            x = id;
                                            break;
                                        default:
                                            x = 0;
                                            break;
                                    }
                                    y = y + 14;
                                }
                                updateWindow();
                                Thread.Sleep(interval);
                            }
                            break;
                        }
                    default:
                        {
                            for (int i = 0; i < spl.Length; i++)
                            {
                                textGraphics.DrawString(spl[i].ToString(), font, fontColor, x, y);
                                x += (int)(textGraphics.MeasureString(spl[i].ToString(), font).Width - 4);
                                x += 4;
                                if (x >= mx)
                                {
                                    switch (autoAdjust)
                                    {
                                        case true:
                                            x = id;
                                            break;
                                        default:
                                            x = 0;
                                            break;
                                    }
                                    y = y + 14;
                                }
                                updateWindow();
                                Thread.Sleep(interval);
                            }
                            break;
                        }
                }

                if (onFinished != null) onFinished();
                triggerWritingFinished();
                return true;
            }
            );
        }

        //Draw a string procedurally at a set interval to the FObject.
        public Task ProceduralDrawString(int x, int y, int interval, string str, bool cleartext, Action onFinished = null)
        {
            this.text = str;
            switch (cleartext)
            {
                case true:
                    ClearText();
                    break;
            }

            string[] spl = str.Split(' ');
            return Task.Run(() =>
            {
                for (int i = 0; i < spl.Length; i++)
                {
                    textGraphics.DrawString(spl[i].ToString(), font, fontColor, x, y);
                    x += (int)(textGraphics.MeasureString(spl[i].ToString(), font).Width - 4);
                    x += 4;
                    if (x >= width - 30)
                    {
                        x = 0;
                        y = y + 14;
                    }
                    updateWindow();
                    Thread.Sleep(interval);
                }
                if (onFinished != null) onFinished();
                triggerWritingFinished();
                return true;
            }
            );
        }

        public void CenterX(screenClass screen, int offset = 0)
        {
            this.x = (screen.windowDimensions[0] / 2) - this.width;
            this.x += offset;
        }

        private void RefreshLoop()
        {
            while (true)
            {
                updateWindow();
            }
        }
    }
}
