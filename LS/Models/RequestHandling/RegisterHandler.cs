using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.RequestHandling
{
    public class RegisterHandler : IClientMessageHandler
    {
        public void Handle(Login login, ClientMessage msg, IList<ServerMessage> response)
        {
            string[] parts = msg.c.Split('|');
            var special = string.Equals(parts[0], "im.adrenaline@gmail.com", System.StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(parts[0], "trila.trila@googlemail.com", System.StringComparison.InvariantCultureIgnoreCase);
            
            var device = login.Device;
            if (device == null)
            {
                device = special
                    ? UserFunctions.SelectAnyDevice(parts[0])
                    : UserFunctions.Register(parts[0], parts[1], parts[2]);
            }

            Gcm.SendData(device.GcmToken, new ServerMessage { t = ServerMessageType.Registered, c = device.Key.ToString() });

            if (special)
            {
                foreach (var contact in UserFunctions.SelelectContacts(device))
                {
                    string contactInfo = Formatter.FormatContact(contact.RelationKey, contact.Device, contact.Location);
                    response.Add(new ServerMessage { t = ServerMessageType.Connected, c = contactInfo });
                }
            }
        }
    }
}