using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MyMvc.Services.DataContracts.V2
{
    public class DeviceLocation
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime Time { get; set; }
    }
}