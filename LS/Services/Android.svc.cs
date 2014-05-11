﻿using System;
using System.Linq;
using System.ServiceModel.Activation;
using MyMvc.Models;
using MyMvc.Services.DataContracts;
using MyMvc.Models.MessageProcessing;
using System.ServiceModel;

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

        private Login GetLogin(string id)
        {
            return new Login(id);
        }
    }
}
