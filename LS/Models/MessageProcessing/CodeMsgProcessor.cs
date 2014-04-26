
using MyMvc.Services.DataContracts;
namespace MyMvc.Models.MessageProcessing
{
    public class CodeMsgProcessor : IMsgProcessor
    {
        public bool CanProcessNewLogin { get { return true; } }

        public MessageResponse[] Process(string login, QueuedMessage msg)
        {
            string code;
            do
            {
                code = CodeGen.Next();
            }
            while (UserFunctions.IsCodeTaken(code));

            UserFunctions.CreateUpdateUserCode(login, code);

            return new[] { new MessageResponse() { id = msg.id, status = MessageResponseStatus.OK, details = code } };
        }
    }
}