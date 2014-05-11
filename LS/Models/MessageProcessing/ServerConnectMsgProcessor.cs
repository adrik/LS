using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerConnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "not_exist";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(Login login, QueuedMessage msg)
        {
            DB.DbUser contact;

            if (UserFunctions.Connect(login.User, msg.content, out contact))
                MsgProcessor.SaveMessageForUser(contact, new QueuedMessage() { content = FormatUserInfo(login.User.Id), type = QueuedMessageType.RequestClientConnect });

            if (contact == null)
                return new[] { MessageResponse.Error(msg.id, NonexistentUserMessage) };
            else
                return new[] { new MessageResponse() { id = msg.id, status = MessageResponseStatus.OK, details = FormatUserInfo(contact.Id) } };
        }

        private string FormatUserInfo(int userId)
        {
            var details =
                    UserFunctions.SelectUser(
                        userId,
                        x => new UserLocation { 
                            id = x.User.Login, 
                            lat = x.Location != null ? x.Location.Lat : 0, 
                            lng = x.Location != null ? x.Location.Lng : 0, 
                            time = x.Location != null ? (DateTime?)x.Location.Time : null }).FirstOrDefault();

            return MsgFormatter.FormatUserLocation(details);
        }
    }
}