﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyMvc.Models.Messages
{
    public class MsgProcessorBrokerV1 : IMsgProcessorBroker
    {
        private IMsgProcessor[] _processors;

        public MsgProcessorBrokerV1()
        {
            _processors = new IMsgProcessor[] {
                new MP_Register() ,
                new MP_Code(),
                new MP_UpdateLocation(),
                new MP_ContactLocations(),
                new MP_ServerConnect(),
                new MP_ServerDisconnect(),
                new MP_ClientConnect(),
                new MP_ClientDisconnect(),
                new MP_Upgrade()
            };
        }


        public IMsgProcessor this[MessageType type]
        {
            get {
                switch (type)
                {
                    case MessageType.Register:
                        return _processors[0];
                    case MessageType.Code:
                        return _processors[1];
                    case MessageType.UpdateLocation:
                        return _processors[2];
                    case MessageType.ContactLocations:
                        return _processors[3];
                    case MessageType.ServerConnect:
                        return _processors[4];
                    case MessageType.ServerDisconnect:
                        return _processors[5];
                    case MessageType.ClientConnect:
                        return _processors[6];
                    case MessageType.ClientDisconnect:
                        return _processors[7];
                    case MessageType.Upgrade:
                        return _processors[8];
                    default:
                        return null;
                }
            }
        }
    }
}