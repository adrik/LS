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
            string other = msg.content.Trim();
            bool disconnected = UserFunctions.Disconnect(login, other);

            if (disconnected)
                MsgProcessor.SaveMessageForUser(other, new QueuedMessage() { content = login, type = QueuedMessageType.RequestClientDisconnect });

            MessageResponse resp = disconnected ? MessageResponse.OK(msg.id) : MessageResponse.Error(msg.id, NonexistentUserMessage);
            return new[] { resp };
        }
    }
}