using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public interface IMsgProcessor
    {
        MessageResponse[] Process(Login login, QueuedMessage msg);
        bool CanProcessNewLogin { get; }
    }
}