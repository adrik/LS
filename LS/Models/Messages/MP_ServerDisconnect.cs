using System;
using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_ServerDisconnect : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            if (login.Device != null)
            {
                try
                {
                    int contactId = int.Parse(msg.c.Trim());
                    var contact = UserFunctions.SelectDevice(contactId);

                    Guid contactRelationKey;
                    bool disconnected = UserFunctions.Disconnect(login.Device, contactId, out contactRelationKey);

                    if (disconnected)
                    {
                        if (contact.GcmToken != null)
                        {
                            var location = UserFunctions.SelectLocation(login.Device.Id);
                            string info = MyMvc.Models.RequestHandling.Formatter.FormatContact(contactRelationKey, login.Device, location);

                            Gcm.SendData(contact.GcmToken, new ServerMessage { t = ServerMessageType.Connected, c = info });
                        }

                        DataMessage message = new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.ClientDisconnect };
                        MessageSystem.SaveMessageForDevice(contactId, message);
                    }
                }
                catch (FormatException) { }
            }
        }
    }
}