using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.DB
{
    public class DbSelection
    {
        public DbUser User { get; set; }
        public DbDevice Device { get; set; }
        public DbLocation Location { get; set; }
    }
}