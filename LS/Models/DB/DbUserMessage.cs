using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Models.DB
{
    public class DbUserMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public MessageType Type { get; set; }
        public string Content { get; set; }
    }
}