using System.Runtime.Serialization;
using MyMvc.Models;

namespace MyMvc.Services.DataContracts
{
    public class MessageResponse
    {
        public static MessageResponse OK(int id)
        {
            return new MessageResponse() { Id = id, Status = MessageResponseStatus.OK };
        }

        public static MessageResponse Error(int id, string details)
        {
            return new MessageResponse() { Id = id, Status = MessageResponseStatus.Error, Details = details };
        }


        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public MessageResponseStatus Status { get; set; }

        [DataMember]
        public string Details { get; set; }
    }
}