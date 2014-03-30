using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.DB
{
    public class DbRelation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public int GroupId { get; set; }
    }
}