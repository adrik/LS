using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerConnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "not_exist";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(Login login, QueuedMessage msg)
        {
            DB.DbDevice contact;

            if (UserFunctions.Connect(login.Device, msg.content, out contact))
                Messages.Messages.SaveMessageForDevice(contact.Id, new DataMessage() { c = login.Device.Id.ToString(), t = MessageType.RequestClientConnect });

            if (contact == null)
                return new[] { MessageResponse.Error(msg.id, NonexistentUserMessage) };
            else
                return new[] { new MessageResponse() { id = msg.id, status = MessageResponseStatus.OK, details = FormatUserInfo(contact.UserId) } };
        }

        public static string FormatUserInfo(int userId)
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