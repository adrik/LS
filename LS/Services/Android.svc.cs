using System;
using System.Linq;
using System.ServiceModel.Activation;
using MyMvc.Models;
using System.ServiceModel;
using MyMvc.Services.DataContracts;
using MyMvc.Services.DataContracts.V2;
using MyMvc.Models.Messages;

namespace MyMvc.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Android : IAndroidService
    {
        public ResponseBag Process(RequestBag request)
        {
            Login login = GetLogin(request.id);

            MessageResponse[] answers = MyMvc.Models.MessageProcessing.MsgProcessor.Process(login, request.msg);
            QueuedMessage[] storedMessages = MyMvc.Models.MessageProcessing.MsgProcessor.GetUserMessages(login);

            return new ResponseBag() { ans = answers, msg = storedMessages };
        }

        public ResponseData Query(RequestData request)
        {
            Login login = new Login(request.i);
            int version = request.v;
            ResponseData response = new ResponseData();

            var allMessages = MessageSystem.GetSavedMessages(login).Union(request.m);
            MPFactory.Instance.GetMP(version).Process(login, allMessages, response);

            return response;
        }

        public ExchangeResponse Exchange(ExchangeRequest request) 
        {
            Login login = new Login(request.i);
            int version = request.v;
            ExchangeResponse response = new ExchangeResponse();

            MPFactory.Instance.RequestHandler.Handle(login, request.m, response);

            return response;
        }

        private Login GetLogin(string id)
        {
            return new Login(id);
        }
    }
}
