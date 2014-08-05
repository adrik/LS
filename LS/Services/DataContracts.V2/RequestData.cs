using System.Runtime.Serialization;

namespace MyMvc.Services.DataContracts.V2
{
    [DataContract]
    public class RequestData
    {
        [DataMember]
        public int i { get; set; }

        [DataMember]
        public int v { get; set; }

        [DataMember]
        public DataMessage[] m { get; set; }
    }
}