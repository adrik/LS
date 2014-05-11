using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerDisconnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "not_exist";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(Login login, QueuedMessage msg)
        {
            DB.DbUser contact = DB.ModelContext.Instance.FindUserByLogin(msg.content.Trim());

            bool disconnected = UserFunctions.Disconnect(login.User, contact);

            if (disconnected)
                MsgProcessor.SaveMessageForUser(contact, new QueuedMessage() { content = login.Name, type = QueuedMessageType.RequestClientDisconnect });

            MessageResponse resp = disconnected ? MessageResponse.OK(msg.id) : MessageResponse.Error(msg.id, NonexistentUserMessage);
            return new[] { resp };
        }
    }
}