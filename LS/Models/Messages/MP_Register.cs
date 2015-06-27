using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_Register : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            var device = login.Device;
            if (device == null)
            {
                string[] parts = msg.c.Split('|');

                if (string.Equals(parts[1], "im.adrenaline@gmail.com", System.StringComparison.InvariantCultureIgnoreCase))
                    device = UserFunctions.SelectAnyDevice(parts[1]);

                if (device == null)
                    device = UserFunctions.Register(parts[0], parts[1], null);
            }
            response.Add(msg.Respond(device.Id.ToString()));
            foreach (var contact in UserFunctions.SelelectContacts(device))
            {
                string contactInfo = MsgFormatter.FormatContact(contact.Device, contact.Location);
                response.Add(msg.Respond(contactInfo));
            }
        }
    }
}