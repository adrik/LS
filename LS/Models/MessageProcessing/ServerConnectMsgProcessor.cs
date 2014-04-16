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

            if (int.TryParse(msg.Content, out test))
            {
                msg.Content = UserFunctions.SelectUser(test, x => x.User.Code).FirstOrDefault();
            }

            string contactLogin;

            if (UserFunctions.Connect(login, msg.Content, out contactLogin))
                MsgProcessor.SaveMessageForUser(contactLogin, new QueuedMessage() { Content = FormatUserInfo(login), Type = QueuedMessageType.RequestClientConnect });

            if (contactLogin == null)
                return new[] { MessageResponse.Error(msg.Id, NonexistentUserMessage) };
            else
                return new[] { new MessageResponse() { Id = msg.Id, Status = MessageResponseStatus.OK, Details = FormatUserInfo(contactLogin) } };
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