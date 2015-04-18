using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages.V2
{
    public class MP_RegisterV2 : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            var device = login.Device;
            if (device == null)
            {
                string[] parts = msg.c.Split('|');

                if (string.Equals(parts[1], "im.adrenaline@gmail.com", System.StringComparison.InvariantCultureIgnoreCase) ||
                    string.Equals(parts[1], "trila.trila@googlemail.com", System.StringComparison.InvariantCultureIgnoreCase))
                    device = UserFunctions.SelectAnyDevice(parts[1]);

                if (device == null)
                    device = UserFunctions.Register(parts[0], parts[1]);
            }
            response.Add(msg.Respond(device.Id.ToString()));
            foreach (var tuple in UserFunctions.SelelectContacts(device))
            {
                string contactInfo = MsgFormatter.FormatContact(tuple.Item1, tuple.Item2);
                response.Add(new DataMessage() { t = MessageType.ClientConnect, c = contactInfo });
            }
        }
    }
}