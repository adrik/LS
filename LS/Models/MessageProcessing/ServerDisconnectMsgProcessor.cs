using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerDisconnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "The user does not exist";

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            bool disconnected = UserFunctions.Disconnect(login, msg.Content.Trim());

            MessageResponse resp = disconnected ? MessageResponse.OK(msg.Id) : MessageResponse.Error(msg.Id, NonexistentUserMessage);
            return new[] { resp };
        }
    }
}