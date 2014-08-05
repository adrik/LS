namespace MyMvc.Models.Messages
{
    public interface IMsgProcessorBroker
    {
        IMsgProcessor this[MessageType type] { get; }
    }
}
