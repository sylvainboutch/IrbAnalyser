using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IrbAnalyser
{
    static class Tools
    {
        public static string cleanStr(string input)
        {
            input = input.ToLowerInvariant();
            input = input.Trim();
            input = input.Replace(" ", "");
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static bool compareStr(string str1, string str2)
        {
            return cleanStr(str1) == cleanStr(str2);
        }
    }
}
