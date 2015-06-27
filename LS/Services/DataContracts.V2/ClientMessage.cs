using System.Runtime.Serialization;
using MyMvc.Models;

namespace MyMvc.Services.DataContracts.V2
{
    [DataContract]
    public class ClientMessage
    {
        [DataMember]
        public ClientMessageType t { get; set; }

        [DataMember]
        public string c { get; set; }
    }
}