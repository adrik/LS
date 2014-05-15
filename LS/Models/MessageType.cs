using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    [Flags]
    public enum MessageType
    {
        ClientConnect = 1,
        ClientDisconnect = 2,
        
        Register = 16,
        Code = 32,
        UpdateLocation = 64,
        ContactLocations = 128,
        ServerConnect = 256,
        ServerDisconnect = 512,
        Upgrade = 1024
    }
}