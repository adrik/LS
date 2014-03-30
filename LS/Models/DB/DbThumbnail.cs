using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.DB
{
    public class DbThumbnail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
}