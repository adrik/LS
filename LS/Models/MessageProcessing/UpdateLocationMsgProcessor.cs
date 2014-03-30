using System;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class UpdateLocationMsgProcessor : IMsgProcessor
    {
        private const string FormatExceptionMessage = "Incorrect Lat Lng format! Should be: {lat},{lng}";

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            try
            {
                double[] latLng = (
                    from str in msg.Content.Split(',')
                    select double.Parse(str.Trim())).ToArray();

                if (latLng.Length != 2)
                    throw new FormatException();

                UserFunctions.UpdateLocation(login, latLng[0], latLng[1]);

                return new[] { MessageResponse.OK(msg.Id) };
            }
            catch (FormatException)
            {
                return new[] { new MessageResponse() { Id = msg.Id, Status = MessageResponseStatus.Error, Details = FormatExceptionMessage } };
            }
        }
    }
}