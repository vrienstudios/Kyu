using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KyuBase.Integrations
{
    public class _PaintEventArgs : EventArgs
    {
        public _PaintEventArgs(object actual)
        {
            Type eventType = actual.GetType();

            if (eventType.Name != "PaintEventArgs")
                throw new ArgumentException("_PaintEventArgs can only take PaintEventArgs");
            List<PropertyInfo> fields = eventType.GetProperties().ToList();
            //FieldInfo f = eventType.GetField("Graphics", BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo field in fields)
            {
                switch (field.Name)
                {
                    case "ClipRectangle":
                        ClipRectangle = (Rectangle)field.GetValue(actual);
                        break;
                    case "Graphics":
                        Graphics = (Graphics)field.GetValue(actual);
                        break;
                }
            }
        }

        public Rectangle ClipRectangle { get; }
        public Graphics Graphics { get; }

        ~_PaintEventArgs()
        {
            Graphics?.Dispose();
            GC.Collect();
        }
    }
}
