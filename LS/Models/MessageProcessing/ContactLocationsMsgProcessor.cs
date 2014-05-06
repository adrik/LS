using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ContactLocationsMsgProcessor : IMsgProcessor
    {
        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            IEnumerable<UserLocation> details = null;
            long androidTime;

            if (msg.content != null && long.TryParse(msg.content, out androidTime) && androidTime > 0)
            {
                DateTime updateTime = MsgFormatter.DateFromAndroid(androidTime);
                
                details =
                    UserFunctions.SelectContacts(
                        login,
                        x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng, time = x.Location.Time },
                        updateTime).ToArray();
            }
            else
            {
                details =
                    UserFunctions.SelectContacts(
                        login,
                        x => new UserLocation
                        {
                            id = x.User.Login,
                            lat = x.Location != null ? x.Location.Lat : 0,
                            lng = x.Location != null ? x.Location.Lng : 0,
                            time = x.Location != null ? (DateTime?)x.Location.Time : null
                        }).ToArray();
            }

            return (
                from d in details
                select new MessageResponse() { id = msg.id, status = MessageResponseStatus.OK, details = MsgFormatter.FormatUserLocation(d) }).ToArray();
        }
    }
}