using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KyuBase.Integrations;

namespace KyuWIN.Extensions
{
    public static class WindowsKyuConverters
    {
        public static _KeyPressEventArgs KeyPressToKyu(this KeyPressEventArgs keys)
            => new _KeyPressEventArgs(keys);

        public static _MouseEventArgs KeyPressToKyu(this MouseEventArgs mice)
            => new _MouseEventArgs(mice);
    }
}
