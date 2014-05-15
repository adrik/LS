using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    public enum QueuedMessageType
    {
        RequestCode = 0,
        RequestUpdateLocation = 1,
        RequestContactLocations = 3,
        RequestServerConnect = 4,
        RequestClientConnect = 5,
        RequestServerDisconnect = 2,
        RequestClientDisconnect = 6
    }
}