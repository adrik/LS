using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    public static class CodeGen
    {
        private static Random rnd = new Random();
        private static long max = 68719476736;

        public static string Next()
        {
            return B64.Create((long)Math.Floor(rnd.NextDouble() * max), 6);
        }
    }
}