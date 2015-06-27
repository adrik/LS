using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models
{
    public enum ClientMessageType
    {
        Register = 0,
        Upgrade = 1,
        GcmTokenUpdate = 2,
        Connect = 3,
        Disconnect = 4,
        CodeRefresh = 5,
        Location = 6,
        ContactUpdates = 7,
        Rate = 8
    }
}