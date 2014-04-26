using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerConnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "The user does not exist";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            int test;

            if (int.TryParse(msg.content, out test))
            {
                msg.content = UserFunctions.SelectUser(test, x => x.User.Code).FirstOrDefault();
            }

            string contactLogin;

            if (UserFunctions.Connect(login, msg.content, out contactLogin))
                MsgProcessor.SaveMessageForUser(contactLogin, new QueuedMessage() { content = FormatUserInfo(login), type = QueuedMessageType.RequestClientConnect });

            if (contactLogin == null)
                return new[] { MessageResponse.Error(msg.id, NonexistentUserMessage) };
            else
                return new[] { new MessageResponse() { id = msg.id, status = MessageResponseStatus.OK, details = FormatUserInfo(contactLogin) } };
        }

        private string FormatUserInfo(string login)
        {
            var details =
                    UserFunctions.SelectUser(
                        login,
                        x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng, time = x.Location.Time }).FirstOrDefault();

            return MsgFormatter.FormatUserLocation(details);
        }
    }
}