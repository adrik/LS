using System;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class UpdateLocationMsgProcessor : IMsgProcessor
    {
        private const string FormatExceptionMessage = "Incorrect Lat Lng format! Should be: {lat}|{lng}|{time}";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            try
            {
                UserLocation location = MsgFormatter.ParseUserLocation(msg.Content);

                UserFunctions.UpdateLocation(login, location.lat, location.lng, location.time);

                return new[] { MessageResponse.OK(msg.Id) };
            }
            catch (FormatException)
            {
                return new[] { new MessageResponse() { Id = msg.Id, Status = MessageResponseStatus.Error, Details = FormatExceptionMessage } };
            }
        }
    }
}