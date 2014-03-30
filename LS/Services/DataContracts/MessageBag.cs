using System.Runtime.Serialization;

namespace MyMvc.Services.DataContracts
{
    [DataContract]
    public class MessageBag
    {
        [DataMember]
        public MessageResponse[] Answers { get; set; }

        [DataMember]
        public QueuedMessage[] Messages { get; set; }
    }
}