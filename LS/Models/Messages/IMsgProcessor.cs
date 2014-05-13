using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.Messages
{
    public interface IMsgProcessor
    {
        void Process(Login login, DataMessage msg, IList<DataMessage> response);
    }
}