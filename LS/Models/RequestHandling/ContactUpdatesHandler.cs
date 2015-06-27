using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.RequestHandling
{
    public class ContactUpdatesHandler : IClientMessageHandler
    {
        public void Handle(Login login, ClientMessage msg, IList<ServerMessage> response)
        {
            if (login.Device == null) return;

            DateTime updateTime = DateTime.UtcNow;

            foreach (var update in UserFunctions.SelectContactUpdates(login.Device))
            {
                string deviceLocation = Formatter.FormatLocation(update.RelationKey, update.Location);
                response.Add(new ServerMessage { t = ServerMessageType.ContactUpdate, c = deviceLocation });
            }

            UserFunctions.SetLastUpdateOnDevice(login.Device, updateTime);            
        }
    }
}