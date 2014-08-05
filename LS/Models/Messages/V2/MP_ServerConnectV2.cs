using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages.V2
{
    public class MP_ServerConnectV2 : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            if (login.Device != null)
            {
                DB.DbDevice contact;

                if (UserFunctions.Connect(login.Device, msg.c, out contact))
                {
                    DataMessage message = new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.ClientConnect };
                    Messages.SaveMessageForDevice(contact.Id, message);
                }

                if (contact != null)
                {
                    string contactInfo = MsgFormatter.FormatContact(contact, UserFunctions.SelectLocation(contact.Id));
                    response.Add(new DataMessage() { t = MessageType.ClientConnect, c = contactInfo });
                }
            }
        }
    }
}