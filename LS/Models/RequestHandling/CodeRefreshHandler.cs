using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.RequestHandling
{
    public class CodeRefreshHandler : IClientMessageHandler
    {
        public void Handle(Login login, ClientMessage msg, IList<ServerMessage> response)
        {
            if (login.Device == null) return;

            string code = UserFunctions.MakeNewCode();
            UserFunctions.SetDeviceCode(login.Device, code);

            response.Add(new ServerMessage { t = ServerMessageType.NewCode, c = code });
        }
    }
}