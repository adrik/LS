using System;
using System.Collections.Generic;
using System.Linq;
using MyMvc.Services.DataContracts;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.MessageProcessing
{
    public class ServerDisconnectMsgProcessor : IMsgProcessor
    {
        private const string NonexistentUserMessage = "not_exist";

        public bool CanProcessNewLogin { get { return false; } }

        public MessageResponse[] Process(Login login, QueuedMessage msg)
        {
            DB.DbUser contact = DB.ModelContext.Instance.FindUserByLogin(msg.content.Trim());
            DB.DbDevice contactDevice = DB.ModelContext.Instance.Devices.FirstOrDefault(x => x.UserId == contact.Id);

            bool disconnected = UserFunctions.Disconnect(login.Device, contactDevice.Id);

            if (disconnected)
                Messages.Messages.SaveMessageForDevice(contactDevice.Id, new DataMessage() { c = login.Name, t = MessageType.RequestClientDisconnect });

            MessageResponse resp = disconnected ? MessageResponse.OK(msg.id) : MessageResponse.Error(msg.id, NonexistentUserMessage);
            return new[] { resp };
        }
    }
}