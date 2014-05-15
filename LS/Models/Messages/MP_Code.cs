using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_Code : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            if (login.Device != null)
            {
                string code = UserFunctions.MakeNewCode();

                UserFunctions.SetDeviceCode(login.Device, code);
                response.Add(msg.Respond(code));
            }
        }
    }
}