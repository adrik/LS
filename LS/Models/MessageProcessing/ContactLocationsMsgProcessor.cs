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
            var details =
                UserFunctions.SelectContacts(
                    login,
                    x => new UserLocation { 
                            id = x.User.Login, 
                            lat = x.Location != null ? x.Location.Lat : 0, 
                            lng = x.Location != null ? x.Location.Lng : 0, 
                            time = x.Location != null ? (DateTime?)x.Location.Time : null }).ToArray();

            return (
                from d in details
                select new MessageResponse() { id = msg.id, status = MessageResponseStatus.OK, details = MsgFormatter.FormatUserLocation(d) }).ToArray();
        }
    }
}