using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Models.DB
{
    public class DbRating
    {
        public int Id { get; set; }
        public int ActorId { get; set; }
        public int Value { get; set; }
        public DateTime Timestamp { get; set; }
    }
}