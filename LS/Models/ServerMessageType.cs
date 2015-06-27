using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    public enum ServerMessageType
    {
        Registered = 0,
        Connected = 1,
        ConnectedAlready = 2,
        BadCode = 3,
        Disconnected = 4,
        ContactUpdate = 5,
        NewCode = 6
    }
}