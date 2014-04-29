using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MyMvc.Models
{
    public static class B64
    {
        private static readonly string symbols = "0123456789abcdefghijk+mnopqrstuvwxyzABCDEFGH$JKLMNOPQRSTUVWXYZ_-";

        public static string Create(long value, int digits = 0)
        {
            int d = 0;
            StringBuilder sb = new StringBuilder();
            while (value > 0 || d < digits)
            {
                sb.Append(symbols[(int)(value % 64)]);
                value >>= 6;
                d++;
            }
            return sb.ToString();
        }

        public static long Parse(string s)
        {
            long result = 0;
            for (int i = 0; i < s.Length; i++)
            {
                result = result << 6 + symbols.IndexOf(s[i]);
            }
            return result;
        }
    }
}