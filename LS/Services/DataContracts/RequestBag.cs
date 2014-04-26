using System.Runtime.Serialization;

namespace MyMvc.Services.DataContracts
{
    [DataContract]
    public class RequestBag
    {
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public QueuedMessage[] msg { get; set; }
    }
}