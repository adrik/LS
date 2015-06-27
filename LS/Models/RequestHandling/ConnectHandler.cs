using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.RequestHandling
{
    public class ConnectHandler : IClientMessageHandler
    {
        public void Handle(Login login, ClientMessage msg, IList<ServerMessage> response)
        {
            if (login.Device == null) return;

            Guid clientRelationKey;
            Guid contactRelationKey;
            DB.DbDevice contact;
            var connected = UserFunctions.Connect(login.Device, msg.c, out contact, out clientRelationKey, out contactRelationKey);
            if (connected)
            {
                if (contact != null && contact.GcmToken != null) {
                    var location = UserFunctions.SelectLocation(login.Device.Id);
                    string info = Formatter.FormatContact(contactRelationKey, login.Device, location);

                    Gcm.SendData(contact.GcmToken, new ServerMessage { t = ServerMessageType.Connected, c = info });
                }

                // TODO: REMOVE LEGACY
                DataMessage message = new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.ClientConnect };
                MyMvc.Models.Messages.MessageSystem.SaveMessageForDevice(contact.Id, message);
                // ===================
            }

            if (contact != null)
            {
                string contactInfo = Formatter.FormatContact(clientRelationKey, contact, UserFunctions.SelectLocation(contact.Id));
                ServerMessageType messageType = connected 
                    ? ServerMessageType.Connected 
                    : ServerMessageType.ConnectedAlready;

                response.Add(new ServerMessage { t = messageType, c = contactInfo });
            }
            else
            {
                response.Add(new ServerMessage() { t = ServerMessageType.BadCode });
            }
        }
    }
}