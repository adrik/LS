using System;
using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MP_ContactLocations : IMsgProcessor
    {
        public void Process(Login login, DataMessage msg, IList<DataMessage> response)
        {
            DateTime updateTime = DateTime.UtcNow;

            foreach (var location in UserFunctions.SelectContactLocations(login.Device))
            {
                string deviceLocation = MsgFormatter.FormatLocation(location);
                response.Add(msg.Respond(deviceLocation));
            }

            login.Device.LastUpdate = updateTime;
            DB.ModelContext.Instance.SaveChanges();
        }
    }
}