using System.Runtime.Serialization;

namespace MyMvc.Services.DataContracts
{
    [DataContract]
    public class ResponseBag
    {
        [DataMember]
        public MessageResponse[] ans { get; set; }

        [DataMember]
        public QueuedMessage[] msg { get; set; }
    }
}