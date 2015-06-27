using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Services.DataContracts;
using MyMvc.Services.DataContracts.V2;

namespace MyMvc.Models.MessageProcessing
{
    public static class MsgProcessor
    {
        private static object msgLock = new object();
        private static UpdateLocationMsgProcessor _updateLocationMP = new UpdateLocationMsgProcessor();
        private static ServerConnectMsgProcessor _serverConnectMP = new ServerConnectMsgProcessor();
        private static ServerDisconnectMsgProcessor _serverDisconnectMP = new ServerDisconnectMsgProcessor();
        private static ContactLocationsMsgProcessor _contactLocationsMP = new ContactLocationsMsgProcessor();
        private static CodeMsgProcessor _codeMP = new CodeMsgProcessor();

        private static IMsgProcessor GetProcessor(QueuedMessageType type)
        {
            switch (type)
            {
                case QueuedMessageType.RequestUpdateLocation:
                    return _updateLocationMP;
                case QueuedMessageType.RequestServerConnect:
                    return _serverConnectMP;
                case QueuedMessageType.RequestServerDisconnect:
                    return _serverDisconnectMP;
                case QueuedMessageType.RequestCode:
                    return _codeMP;
                case QueuedMessageType.RequestContactLocations:
                    return _contactLocationsMP;
                default:
                    throw new ArgumentException();
            }
        }

        private static MessageResponse[] Process(Login login, QueuedMessage msg)
        {
            IMsgProcessor processor = GetProcessor(msg.type);

            if (login.Device != null || processor.CanProcessNewLogin)
                return processor.Process(login, msg);
            else
                return new[] { MessageResponse.Error(msg.id, "User is not registered") };
        }

        public static MessageResponse[] Process(Login login, QueuedMessage[] messages)
        {
            // filter messages and take only last location update
            var locationUpdates = messages.Where(x => x.type == QueuedMessageType.RequestUpdateLocation).ToArray();

            List<QueuedMessage> filtered = messages.Where(x => x.type != QueuedMessageType.RequestUpdateLocation).ToList();
            if (locationUpdates.Length > 0)
                filtered.Add(locationUpdates.Last());

            var result = filtered.SelectMany(x => Process(login, x));
            if (locationUpdates.Length > 1)
                result = result.Union(locationUpdates.Take(locationUpdates.Length - 1).Select(x => MessageResponse.OK(x.id)));

            return result.ToArray();
        }

        public static QueuedMessage[] GetUserMessages(Login login)
        {
            DataMessage[] messages = Messages.MessageSystem.GetSavedMessages(login);
            List<QueuedMessage> result = new List<QueuedMessage>();

            foreach (DataMessage msg in messages)
            {
                int deviceId = int.Parse(msg.c);
                var device = UserFunctions.SelectDevice(deviceId);

                switch (msg.t)
                {
                    case MessageType.ClientConnect:
                        result.Add(new QueuedMessage()
                        {
                            id = 0,
                            content = ServerConnectMsgProcessor.FormatUserInfo(device.UserId),
                            type = QueuedMessageType.RequestClientConnect
                        });
                        break;
                    case MessageType.ClientDisconnect:
                        result.Add(new QueuedMessage()
                        {
                            id = 0,
                            content = UserFunctions.SelectUser(device.UserId).Login,
                            type = QueuedMessageType.RequestClientDisconnect
                        });
                        break;
                }
            }

            return result.ToArray();
        }
    }
}