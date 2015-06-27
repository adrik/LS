using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.RequestHandling
{
    public class DisconnectHandler : IClientMessageHandler
    {
        public void Handle(Login login, ClientMessage msg, IList<ServerMessage> response)
        {
            if (login.Device == null) return;
             
            try
            {
                var relationKey = Guid.Parse(msg.c.Trim());
                var contact = UserFunctions.FindContact(login.Device, relationKey);

                if (contact == null) return;

                Guid contactRelationKey;
                bool disconnected = UserFunctions.Disconnect(login.Device, contact.Id, out contactRelationKey);

                if (disconnected)
                {
                    if (contact.GcmToken != null)
                    {
                        var location = UserFunctions.SelectLocation(login.Device.Id);
                        string info = Formatter.FormatContact(contactRelationKey, login.Device, location);

                        Gcm.SendData(contact.GcmToken, new ServerMessage { t = ServerMessageType.Connected, c = info });
                    }

                    // TODO: REMOVE LEGACY
                    DataMessage message = new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.ClientDisconnect };
                    MyMvc.Models.Messages.MessageSystem.SaveMessageForDevice(contact.Id, message);
                    // ===================
                }
            }
            catch (FormatException) { }
        }
    }
}