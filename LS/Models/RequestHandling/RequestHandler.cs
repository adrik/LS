using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Models.RequestHandling;

namespace MyMvc.Models
{
    public class RequestHandler
    {
        private IClientMessageHandler[] _handlers = new IClientMessageHandler[]
        {
            new RegisterHandler(),
            new UpgradeHandler(),
            new GcmTokenUpdateHandler(),
            new ConnectHandler(),
            new DisconnectHandler(),
            new CodeRefreshHandler(),
            new LocationHandler(),
            new ContactUpdatesHandler(),
            new RateHandler()
        };

        public void Handle(Login login, IEnumerable<ClientMessage> messages, IList<ServerMessage> response)
        {
            foreach (ClientMessage message in messages) {
                _handlers[(int)message.t].Handle(login, message, response);
            }
        }
    }
}