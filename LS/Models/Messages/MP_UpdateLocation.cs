using System;
using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_UpdateLocation : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            if (login.Device != null)
            {
                try
                {
                    var location = MsgFormatter.ParseLocation(msg.c);
                    UserFunctions.UpdateLocation(login.Device.Id, location);
                }
                catch (FormatException) { }
            }
        }
    }
}