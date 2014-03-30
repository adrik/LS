using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Services.DataContracts;

namespace MyMvc.Models.MessageProcessing
{
    public static class MsgProcessor
    {
        private static UpdateLocationMsgProcessor _updateLocationMP = new UpdateLocationMsgProcessor();
        private static ServerConnectMsgProcessor _serverConnectMP = new ServerConnectMsgProcessor();
        private static ServerDisconnectMsgProcessor _serverDisconnectMP = new ServerDisconnectMsgProcessor();
        private static ContactLocationsMsgProcessor _contactLocationsMP = new ContactLocationsMsgProcessor();
        private static CodeMsgProcessor _codeMP = new CodeMsgProcessor();

        private static IMsgProcessor GetProcessor(MessageType type)
        {
            switch (type)
            {
                case MessageType.RequestUpdateLocation:
                    return _updateLocationMP;
                case MessageType.RequestServerConnect:
                    return _serverConnectMP;
                case MessageType.RequestServerDisconnect:
                    return _serverDisconnectMP;
                case MessageType.RequestCode:
                    return _codeMP;
                case MessageType.RequestContactLocations:
                    return _contactLocationsMP;
                default:
                    throw new ArgumentException();
            }
        }

        public static MessageResponse[] Process(string login, QueuedMessage msg)
        {
            return GetProcessor(msg.Type).Process(login, msg);
        }

        public static MessageResponse[] Process(string login, QueuedMessage[] messages)
        {
            return messages.SelectMany(x => Process(login, x)).ToArray();
        }

        public static void SaveMessageForUser(string login, QueuedMessage msg)
        {
            var db = Models.DB.ModelContext.Instance;
            var user = db.FindUserByLogin(login);

            db.UserMessages.Add(new DB.DbUserMessage() { UserId = user.Id, Type = msg.Type, Content = msg.Content });
        }

        public static QueuedMessage[] GetUserMessages(string login)
        {
            var db = Models.DB.ModelContext.Instance;
            var user = db.FindUserByLogin(login);

            DB.DbUserMessage[] messages = db.UserMessages.Where(x => x.UserId == user.Id).ToArray();
            foreach (var msg in messages)
                db.UserMessages.Remove(msg);
            db.SaveChanges();

            return messages.Select(x => new QueuedMessage() { Id = x.Id, Content = x.Content, Type = x.Type }).ToArray();
        }
    }
}