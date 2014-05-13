using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.Messages
{
    public class MsgProcessorBroker
    {
        private static Lazy<MsgProcessorBroker> _instance =
            new Lazy<MsgProcessorBroker>(() => new MsgProcessorBroker());

        public static MsgProcessorBroker Instance { get { return _instance.Value; } }


        private IMsgProcessor[] _processors;

        private MsgProcessorBroker()
        {
            _processors = new IMsgProcessor[] {
                new MP_Code(),
                new MP_UpdateLocation(),
                new MP_ServerDisconnect(),
                new MP_ContactLocations(),
                new MP_ServerConnect(),
                new MP_ClientConnect(),
                new MP_ClientDisconnect() };

        }


        public IMsgProcessor this[MessageType type]
        {
            get { return _processors[(int)type]; }
        }
    }
}