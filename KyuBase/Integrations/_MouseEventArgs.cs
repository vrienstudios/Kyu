using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KyuBase.Integrations
{
    //An exact replica of System.Windows.Forms.MouseButtons
    [Flags]
    public enum _MouseButtons
    {
        None = 0,
        Left = 1048576,
        Right = 2097152,
        Middle = 4194304,
        XButton1 = 8388608,
        XButton2 = 16777216
    }

    public class _MouseEventArgs : EventArgs
    {
        public _MouseEventArgs(_MouseButtons button, int clicks, int x, int y, int delta)
        {
            Button = button;
            Clicks = clicks;
            X = x;
            Y = y;
            Delta = delta;
            Location = new Point(x, y);
        }

        public _MouseEventArgs(object actual)
        {
            Type eventType = actual.GetType();
            if (eventType.Name != "MouseEventArgs")
                throw new ArgumentException("_MouseEventArgs can only take MouseEventArgs");
            List<FieldInfo> fields = eventType.GetFields().ToList();
            foreach (FieldInfo field in fields)
            {
                switch (field.Name)
                {
                    case "Button":
                        Button = (_MouseButtons)((int)field.GetValue(actual));
                        break;
                    case "Clicks":
                        Clicks = (int)field.GetValue(actual);
                        break;
                    case "X":
                        X = (int)field.GetValue(actual);
                        break;
                    case "Y":
                        Y = (int)field.GetValue(actual);
                        break;
                    case "Delta":
                        Delta = (int)field.GetValue(actual);
                        break;
                    case "Location":
                        Location = (Point)field.GetValue(actual);
                        break;
                }
            }
        }

        public _MouseButtons Button { get; }
        public int Clicks { get; }
        public int X { get; }
        public int Y { get; }
        public int Delta { get; }
        public Point Location { get; }
    }
}
