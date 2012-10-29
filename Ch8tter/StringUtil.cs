using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch8tter
{
    class StringUtil
    {
        public static string TruncateAtWord(string value, int length)
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", length))+"...";
        }
    }
}
