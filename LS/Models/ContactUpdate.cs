using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    public class ContactUpdate
    {
        public Guid RelationKey { get; set; }
        public DB.DbDevice Device { get; set; }
        public DB.DbLocation Location { get; set; }
    }
}