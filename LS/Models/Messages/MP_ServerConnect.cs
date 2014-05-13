using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_ServerConnect : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            DB.DbDevice contact;

            if (UserFunctions.Connect(login.Device, msg.c, out contact))
            {
                DataMessage message = new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.RequestClientConnect };
                Messages.SaveMessageForDevice(contact.Id, message);
            }

            if (contact != null)
            {
                string contactInfo = MsgFormatter.FormatContact(contact, UserFunctions.SelectLocation(contact.Id));
                response.Add(msg.Respond(contactInfo));
            }
        }
    }
}