using System.Runtime.Serialization;
using MyMvc.Models;

namespace MyMvc.Services.DataContracts.V2
{
    [DataContract]
    public class DataMessage
    {
        [DataMember]
        public MessageType t { get; set; }

        [DataMember]
        public string c { get; set; }


        public DataMessage Respond(string content)
        {
            return new DataMessage() { t = this.t, c = content };
        }
    }
}