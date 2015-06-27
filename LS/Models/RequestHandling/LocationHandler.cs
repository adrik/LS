using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.RequestHandling
{
    public class LocationHandler : IClientMessageHandler
    {
        public void Handle(Login login, ClientMessage msg, IList<ServerMessage> response)
        {
            if (login.Device == null) return;
             
            try
            {
                var location = Formatter.ParseLocation(msg.c);
                UserFunctions.UpdateLocation(login.Device.Id, location);
            }
            catch (FormatException) { }
        }
    }
}