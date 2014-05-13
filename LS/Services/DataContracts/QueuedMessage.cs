using System.Runtime.Serialization;
using MyMvc.Models;

namespace MyMvc.Services.DataContracts
{
    [DataContract]
    public class QueuedMessage
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public MessageType type { get; set; }

        [DataMember]
        public string content { get; set; }
    }
}