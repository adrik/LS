using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public class MsgProcessor : IBulkMsgProcessor
    {
        private IMsgProcessorBroker _broker;

        public MsgProcessor(IMsgProcessorBroker broker)
        {
            _broker = broker;
        }

        public void Process(Login login, IEnumerable<Services.DataContracts.V2.DataMessage> messages, IList<Services.DataContracts.V2.DataMessage> response)
        {
            foreach (DataMessage msg in messages)
            {
                IMsgProcessor processor = _broker[msg.t];
                processor.Process(login, msg, response);
            }
        }
    }
}