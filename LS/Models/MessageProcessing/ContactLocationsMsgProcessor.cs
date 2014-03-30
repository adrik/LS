using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ContactLocationsMsgProcessor : IMsgProcessor
    {
        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            var details =
                UserFunctions.SelectContacts(
                    login,
                    x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng }).ToArray();

            return (
                from d in details
                let detailsStr = string.Format("{0}|{1}|{2}", d.id, d.lat, d.lng)
                select new MessageResponse() { Id = msg.Id, Status = MessageResponseStatus.OK, Details = detailsStr }).ToArray();
        }
    }
}