using System;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class UpdateLocationMsgProcessor : IMsgProcessor
    {
        private const string FormatExceptionMessage = "bad_format";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(Login login, QueuedMessage msg)
        {
            try
            {
                UserLocation location = MsgFormatter.ParseUserLocation(msg.content);

                UserFunctions.UpdateLocation(login.User.Id, location.lat, location.lng, location.time.Value);

                return new[] { MessageResponse.OK(msg.id) };
            }
            catch (FormatException)
            {
                return new[] { new MessageResponse() { id = msg.id, status = MessageResponseStatus.Error, details = FormatExceptionMessage } };
            }
        }
    }
}