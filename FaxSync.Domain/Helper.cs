using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaxSync.Domain
{
    public static class Helper
    {
        public static int Compare(this User x, User y)
        {
            if (x == null && y == null)
                return 0;

            else if (x == null || y == null)
                return -1;
            else
                return x.CompareTo(y);

        }
        public static int Compare(this FaxNumber x, FaxNumber y)
        {
            if (x == null && y == null)
                return 0;

            else if (x == null || y == null)
                return -1;
            else
                return x.CompareTo(y);

        }
        public static bool CompareAreEqual(this string str1,string str2)
        {
            return string.Compare(str1,str2,true) == 0;
        }
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static bool IsNotEmpty(this string str)
        {
            return !str.IsEmpty();
        }

        public static bool NotNull(this object obj)
        {
            return obj != null;
        }

        public static bool Not(this bool obj)
        {
            return !obj;
        }
        public static int toInt(this string obj)
        {
            return int.Parse(obj);
        }
    }
}
