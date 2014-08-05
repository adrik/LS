using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public interface IBulkMsgProcessor
    {
        void Process(Login login, IEnumerable<DataMessage> messages, IList<DataMessage> response);
    }
}
