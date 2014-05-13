using System;
using System.Linq;
using System.ServiceModel.Activation;
using MyMvc.Models;
using System.ServiceModel;
using MyMvc.Services.DataContracts;
using MyMvc.Services.DataContracts.V2;
using MyMvc.Models.MessageProcessing;
using MyMvc.Models.Messages;

namespace MyMvc.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Android : IAndroidService
    {
        public ResponseBag Process(RequestBag request)
        {
            Login login = GetLogin(request.id);

            MessageResponse[] answers = MsgProcessor.Process(login, request.msg);
            QueuedMessage[] storedMessages = MsgProcessor.GetUserMessages(login);

            return new ResponseBag() { ans = answers, msg = storedMessages };
        }

        public ResponseData Query(RequestData request)
        {
            Login login = new Login(int.Parse(request.i));
            ResponseData response = new ResponseData();

            var allMessages = Messages.GetSavedMessages(login).Union(request.m);
            Messages.Process(login, allMessages, response);

            return response;
        }

        private Login GetLogin(string id)
        {
            return new Login(id);
        }
    }
}
