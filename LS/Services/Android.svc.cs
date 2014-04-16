using System;
using System.Linq;
using System.ServiceModel.Activation;
using MyMvc.Models;
using MyMvc.Services.DataContracts;
using MyMvc.Models.MessageProcessing;
using System.ServiceModel;

namespace MyMvc.Services
{
    [ServiceBehavior(Namespace = "octo.users.port", Name = "UserPort")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Android : IAndroidService
    {
        public MessageBag Process(string id, QueuedMessage[] messages)
        {
            string login = GetLogin(id);

            MessageResponse[] answers = MsgProcessor.Process(login, messages);
            QueuedMessage[] storedMessages = MsgProcessor.GetUserMessages(login);

            return new MessageBag() { Answers = answers, Messages = storedMessages };
        }

        private string GetLogin(string id)
        {
            return id;
        }
    }
}
