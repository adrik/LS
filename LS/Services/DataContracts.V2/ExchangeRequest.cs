using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MyMvc.Services.DataContracts.V2
{
    [DataContract]
    public class ExchangeRequest
    {
        [DataMember]
        public Guid i { get; set; }

        [DataMember]
        public int v { get; set; }

        [DataMember]
        public ClientMessage[] m { get; set; }
    }
}