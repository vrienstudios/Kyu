using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KyuBase.Integrations
{
    public class _KeyPressEventArgs : EventArgs
    {
        public _KeyPressEventArgs(char keyChar, bool handled = false)
        {
            KeyChar = KeyChar;
            Handled = handled;
        }

        public _KeyPressEventArgs(object actual)
        {
            Type eventType = actual.GetType();
            if (eventType.Name != "KeyPressEventArgs")
                throw new ArgumentException("_KeyPressEventArgs can only take KeyPressEventArg");
            List<PropertyInfo> fields = eventType.GetProperties().ToList();
            foreach(PropertyInfo field in fields)
            {
                if (field.Name == "KeyChar")
                    KeyChar = (char)field.GetValue(actual);
                else if (field.Name == "Handled")
                    Handled = (bool)field.GetValue(actual);
            }
        }

        public char KeyChar { get; set; }
        public bool Handled { get; set; }
    }
}
