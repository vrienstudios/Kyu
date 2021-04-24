using KyuBase.Integrations;
using KyuBase.UIElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace KyuBase.Objects
{
    public abstract class FHandle
    {
        ManualResetEvent resetEvent;

        public Thread fThr;
        public Thread refresher;

        public List<FObject> FObjects;
        private FObject focused;

        public delegate void formClosed();
        public event formClosed onFormClose;

        public delegate void KeyPress(_KeyPressEventArgs e);
        public event KeyPress onKeyPress;

        public delegate void MouseClick(_MouseEventArgs e);
        public event MouseClick onMouseClick;

        #region Properties
        // Offsets to provide a perfect alignment with the window.
        public int xOffset;
        public int yOffset;

        public int Height;
        public int Width;

        public string Title;

        public bool Sizable;
        #endregion

        public FHandle(int height, int width, Point offSets, string title, bool sizable)
        {
            FObjects = new List<FObject>();
            Height = height;
            Width = width;
            Title = title;
            Sizable = sizable;

            yOffset = offSets.Y;
            xOffset = offSets.X;
        }

        public void AddFObject(FObject fob)
        {
            FObjects.Add(fob);
            FObjects.Sort((x, y) => y.hierachy.ToString().CompareTo(x.hierachy.ToString()));
        }

        public void Freeze()
        {
            if (fThr.IsAlive)
                resetEvent.Reset();
        }

        public void formClosing(object sender, _FormClosingEventArgs e)
        {
            onFormClose?.Invoke();
        }

        public void formKeyPress(object sender, _KeyPressEventArgs e)
        {
            if (focused?.GetType() == typeof(TextInput))
                ((TextInput)focused).AddCharacter(e.KeyChar);
            else
                onKeyPress?.Invoke(e);
        }

        public void formPaint(object sender, _PaintEventArgs e)
        {
            A:
            try
            {
                foreach (FObject foo in FObjects)
                    e.Graphics.DrawImage(foo.window, foo.x, foo.y);
            }
            catch
            {
                goto A;
            }
        }

        public void handlePaint(object sender, _PaintEventArgs e)
        {
            for (int idx = (FObjects.Count() - 1); idx > -1; idx--)
                e.Graphics.DrawImage(FObjects[idx].window, FObjects[idx].x, FObjects[idx].y);
        }

        public void formMouseMove(object sender, _MouseEventArgs e)
        {
            try
            {
                FObject a = FObjects.First(f => f.x <= e.X && e.X - f.x <= f.window.Width && f.y <= e.Y && e.Y - f.y <= f.window.Height);
                if (focused == a)
                {
                    focused?.callEvnt(Evnts.OnMouseHover, e.X, e.Y);
                    return;
                }
                focused?.callEvnt(Evnts.OnMouseLeave, 0, 0);
                focused = a;
                focused?.callEvnt(Evnts.OnMouseHover, e.X, e.Y);
            }
            catch (Exception ex)
            {
                Console.WriteLine(e.X);
                Console.WriteLine(ex.Message);
                GC.Collect();
            }
        }

        public void formMouseClick(object sender, _MouseEventArgs e)
        {
            onMouseClick?.Invoke(e);
            if (e.Button == _MouseButtons.Left)
                try
                {
                    focused = FObjects.First(f => f.x <= e.X && e.X - f.x <= f.window.Width && f.y <= e.Y && e.Y - f.y <= f.window.Height);
                    focused.callEvnt(Evnts.OnClick, e.X - focused.x, e.Y);
                }
                catch
                {
                    focused = null;
                }
        }

        public abstract void formRun();
        public abstract void refresh();
    }
}
