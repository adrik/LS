using System.Runtime.Serialization;
using MyMvc.Models;

namespace MyMvc.Services.DataContracts
{
    [DataContract]
    public class QueuedMessage
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public MessageType Type { get; set; }

        [DataMember]
        public string Content { get; set; }
    }
}