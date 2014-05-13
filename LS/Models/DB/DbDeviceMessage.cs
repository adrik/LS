using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.DB
{
    public class DbDeviceMessage
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
    }
}