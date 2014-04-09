using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerDisconnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "The user does not exist";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            string other = msg.Content.Trim();
            bool disconnected = UserFunctions.Disconnect(login, other);

            if (disconnected)
                MsgProcessor.SaveMessageForUser(other, new QueuedMessage() { Content = login, Type = QueuedMessageType.RequestClientDisconnect });

            MessageResponse resp = disconnected ? MessageResponse.OK(msg.Id) : MessageResponse.Error(msg.Id, NonexistentUserMessage);
            return new[] { resp };
        }
    }
}