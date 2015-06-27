using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    public class GcmMessage
    {
        public string to { get; set; }
        public ServerMessage data { get; set; }

        public GcmMessage(string to, ServerMessage data)
        {
            this.to = to;
            this.data = data;
        }
    }
}