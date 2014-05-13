using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.DB
{
    public class DbDeviceRelation
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int OtherDeviceId { get; set; }
        public int GroupId { get; set; }
    }
}