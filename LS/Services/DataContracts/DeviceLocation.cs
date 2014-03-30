using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Services.DataContracts
{
    public class DeviceLocation
    {
        public int id { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
    }
}