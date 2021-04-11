using System;
using System.Collections.Generic;
using System.Text;

namespace KyuBase.Objects
{
    public class SList : List<String>
    {
        public static bool operator +(SList aa, string a)
        {
            aa.Add(a);
            return true;
        }
    }
}
