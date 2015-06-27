using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;
using System;

namespace MyMvc.Models.Messages.V2
{
    public class MP_ServerConnectV2 : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            if (login.Device != null)
            {
                Guid clientRelationKey;
                Guid contactRelationKey;
                DB.DbDevice contact;
                var connected = UserFunctions.Connect(login.Device, msg.c, out contact, out clientRelationKey, out contactRelationKey);

                if (connected)
                {
                    if (contact.GcmToken != null)
                    {
                        var location = UserFunctions.SelectLocation(login.Device.Id);
                        string info = MyMvc.Models.RequestHandling.Formatter.FormatContact(contactRelationKey, login.Device, location);

                        Gcm.SendData(contact.GcmToken, new ServerMessage { t = ServerMessageType.Connected, c = info });
                    }

                    DataMessage message = new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.ClientConnect };
                    MessageSystem.SaveMessageForDevice(contact.Id, message);
                }

                if (contact != null)
                {
                    string contactInfo = MsgFormatter.FormatContact(contact, UserFunctions.SelectLocation(contact.Id));
                    MessageType messageType = connected ? MessageType.ClientConnect : MessageType.AlreadyConnected;
                    response.Add(new DataMessage() { t = messageType, c = contactInfo });
                }
                else
                {
                    response.Add(new DataMessage() { t = MessageType.BadCode });
                }
            }
        }
    }
}