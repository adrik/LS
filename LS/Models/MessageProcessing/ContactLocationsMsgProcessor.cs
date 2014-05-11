using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ContactLocationsMsgProcessor : IMsgProcessor
    {
        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(Login login, QueuedMessage msg)
        {
            DateTime updateTime = DateTime.UtcNow;

            var details =
                UserFunctions.SelectContactsForDevice(
                    login.Device,
                    x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng, time = x.Location.Time }).ToArray();

            login.Device.LastUpdate = updateTime;
            DB.ModelContext.Instance.SaveChanges();

            return (
                from d in details
                select new MessageResponse() { id = msg.id, status = MessageResponseStatus.OK, details = MsgFormatter.FormatUserLocation(d) }).ToArray();
        }
    }
}