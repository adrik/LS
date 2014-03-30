using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Models.DB
{
    public class DbUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Code { get; set; }
    }
}