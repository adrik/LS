using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Services.DataContracts
{
    public class DeviceInfo : DeviceLocation
    {
        public int uid { get; set; }
        public string uname { get; set; }
        public string dname { get; set; }
    }
}