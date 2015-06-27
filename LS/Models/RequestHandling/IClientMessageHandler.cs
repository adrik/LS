using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyMvc.Models.RequestHandling
{
    public interface IClientMessageHandler
    {
        void Handle(Login login, ClientMessage msg, IList<ServerMessage> response);
    }
}
