using KyuBase.Integrations;
using KyuBase.UIElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace KyuBase.Objects
{
    public abstract class FHandle
    {
        ManualResetEvent resetEvent;

        Thread fThr;
        Thread refresher;

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

        public abstract void formRun();
        public abstract void refresh();
    }
}
