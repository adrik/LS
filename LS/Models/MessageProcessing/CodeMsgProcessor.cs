
using MyMvc.Services.DataContracts;
namespace MyMvc.Models.MessageProcessing
{
    public class CodeMsgProcessor : IMsgProcessor
    {
        public bool CanProcessNewLogin { get { return true; } }

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            string code = CodeGen.Next();
            UserFunctions.CreateUpdateUserCode(login, code);

            return new[] { new MessageResponse() { Id = msg.Id, Status = MessageResponseStatus.OK, Details = code } };
        }
    }
}