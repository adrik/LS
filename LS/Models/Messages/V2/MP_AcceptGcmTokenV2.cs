using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages.V2
{
    public class MP_AcceptGcmTokenV2 : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            if (login.Device != null)
                UserFunctions.SetGcmToken(login.Device, msg.c);
        }
    }
}