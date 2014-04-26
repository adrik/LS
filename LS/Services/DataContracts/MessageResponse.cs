using System.Runtime.Serialization;
using MyMvc.Models;

namespace MyMvc.Services.DataContracts
{
    public class MessageResponse
    {
        public static MessageResponse OK(int id)
        {
            return new MessageResponse() { id = id, status = MessageResponseStatus.OK };
        }

        public static MessageResponse Error(int id, string details)
        {
            return new MessageResponse() { id = id, status = MessageResponseStatus.Error, details = details };
        }


        [DataMember]
        public int id { get; set; }

        [DataMember]
        public MessageResponseStatus status { get; set; }

        [DataMember]
        public string details { get; set; }
    }
}