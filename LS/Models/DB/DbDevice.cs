using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvc.Models.DB
{
    public class DbDevice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string DeviceKey { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string GcmToken { get; set; }
    }
}