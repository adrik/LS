using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_ClientConnect : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            int deviceId = int.Parse(msg.c);
            var device = UserFunctions.SelectDevice(deviceId);
            var location = UserFunctions.SelectLocation(deviceId);

            string info = MsgFormatter.FormatContact(device, location);
            response.Add(msg.Respond(info));
        }
    }
}