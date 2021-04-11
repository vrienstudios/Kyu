using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KyuBase.Integrations
{

    public enum _CloseReason
    {
        None = 0,
        WindowsShutDown = 1,
        MdiFormClosing = 2,
        UserClosing = 3,
        TaskManagerClosing = 4,
        FormOwnerClosing = 5,
        ApplicationExitCall = 6
    }
    public class _FormClosingEventArgs : EventArgs
    {
        public _FormClosingEventArgs(object o)
        {
            Type t = o.GetType();
            List<FieldInfo> fields = t.GetFields().ToList();
            foreach(FieldInfo f in fields)
            {
                if (f.Name == "CloseReason")
                    CloseReason = (_CloseReason)(int)f.GetValue(o);
            }
        }

        public _CloseReason CloseReason { get; }
    }
}
