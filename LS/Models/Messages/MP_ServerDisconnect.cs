using System;
using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_ServerDisconnect : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            try
            {
                int contactId = int.Parse(msg.c.Trim());
                bool disconnected = UserFunctions.Disconnect(login.Device, contactId);

                if (disconnected)
                {
                    DataMessage message = new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.RequestClientDisconnect };
                    Messages.SaveMessageForDevice(contactId, message);
                }
            }
            catch (FormatException) { }
        }
    }
}