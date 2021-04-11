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
        public static _KeyPressEventArgs ToKyu(this KeyPressEventArgs keys)
            => new _KeyPressEventArgs(keys);

        public static _MouseEventArgs ToKyu(this MouseEventArgs mice)
            => new _MouseEventArgs(mice);

        public static _FormClosingEventArgs ToKyu(this FormClosingEventArgs mice)
            => new _FormClosingEventArgs(mice);

        public static _PaintEventArgs ToKyu(this PaintEventArgs e)
            => new _PaintEventArgs(e);
    }
}
