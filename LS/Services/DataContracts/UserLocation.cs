using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MyMvc.Services.DataContracts
{
    [DataContract]
    public class UserLocation
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public double lat { get; set; }

        [DataMember]
        public double lng { get; set; }

        [DataMember]
        public DateTime? time { get; set; }
    }
}