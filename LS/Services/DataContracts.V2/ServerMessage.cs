using System.Runtime.Serialization;
using MyMvc.Models;

namespace MyMvc.Services.DataContracts.V2
{
    [DataContract]
    public class ServerMessage
    {
        [DataMember]
        public ServerMessageType t { get; set; }

        [DataMember]
        public string c { get; set; }
    }
}