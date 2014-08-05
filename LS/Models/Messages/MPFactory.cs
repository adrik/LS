using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Models.Messages.V2;

namespace MyMvc.Models.Messages
{
    public class MPFactory
    {
        private static Lazy<MPFactory> _instance = new Lazy<MPFactory>(() => new MPFactory());
        private IBulkMsgProcessor[] _processors;

        private MPFactory()
        {
            _processors = new[] { new MsgProcessor(new MsgProcessorBrokerV1()), null, new MsgProcessor(new MsgProcessorBrokerV2()) };
        }

        public static MPFactory Instance
        {
            get { return _instance.Value; }
        }

        public IBulkMsgProcessor GetMP(int version)
        {
            return _processors[version];
        }
    }
}