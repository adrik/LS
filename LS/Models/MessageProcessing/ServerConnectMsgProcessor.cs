using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerConnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "The user does not exist";

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            int test;

            if (int.TryParse(msg.Content, out test) && test > 0 && test < 7)
            {
                msg.Content = UserFunctions.SelectUser(test, x => x.User.Code).FirstOrDefault();
            }

            string contactLogin;

            if (UserFunctions.Connect(login, msg.Content, out contactLogin))
                MsgProcessor.SaveMessageForUser(contactLogin, new QueuedMessage() { Content = login, Type = MessageType.RequestClientConnect });

            if (contactLogin == null)
                return new[] { MessageResponse.Error(msg.Id, NonexistentUserMessage) };
            else
            {
                var details =
                    UserFunctions.SelectUser(
                        contactLogin,
                        x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng }).FirstOrDefault();

                string detailsStr = string.Format("{0}|{1}|{2}", details.id, details.lat, details.lng);

                return new[] { new MessageResponse() { Id = msg.Id, Status = MessageResponseStatus.OK, Details = detailsStr } };
            }
        }
    }
}