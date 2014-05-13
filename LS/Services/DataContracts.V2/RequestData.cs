using System.Runtime.Serialization;

namespace MyMvc.Services.DataContracts.V2
{
    [DataContract]
    public class RequestData
    {
        [DataMember]
        public string i { get; set; }

        [DataMember]
        public DataMessage[] m { get; set; }
    }
}