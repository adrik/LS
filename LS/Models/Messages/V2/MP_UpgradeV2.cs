using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages.V2
{
    public class MP_UpgradeV2 : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            string[] parts = msg.c.Split('|');
            int version = int.Parse(parts[0]);

            if (version == 4)
            {
                var device = login.Device;
                if (device == null)
                    device = UserFunctions.SelectAnyDevice(parts[1]);

                response.Add(new DataMessage() { t = MessageType.Register, c = device.Id.ToString() });
                response.Add(new DataMessage() { t = MessageType.Code, c = device.Code });
                foreach (var tuple in UserFunctions.SelelectContacts(device))
                {
                    string contactInfo = MsgFormatter.FormatContact(tuple.Item1, tuple.Item2);
                    response.Add(new DataMessage() { t = MessageType.ClientConnect, c = contactInfo });
                }
            }
        }
    }
}