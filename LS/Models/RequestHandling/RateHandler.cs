using System.Collections.Generic;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.RequestHandling
{
    public class RateHandler : IClientMessageHandler
    {
        public void Handle(Login login, ClientMessage msg, IList<ServerMessage> response)
        {
            if (login.Device != null)
                UserFunctions.SaveUserRating(login.Device, int.Parse(msg.c));
        }
    }
}