using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMvc.Services.DataContracts;

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

        private static MessageResponse[] Process(string login, QueuedMessage msg, bool userExists)
        {
            IMsgProcessor processor = GetProcessor(msg.type);

            if (userExists || processor.CanProcessNewLogin)
                return processor.Process(login, msg);
            else
                return new[] { MessageResponse.Error(msg.id, "User is not registered") };
        }

        public static MessageResponse[] Process(string login, QueuedMessage[] messages)
        {
            var db = DB.ModelContext.Instance;
            var user = db.FindUserByLogin(login);

            return messages.SelectMany(x => Process(login, x, user != null)).ToArray();
        }

        public static void SaveMessageForUser(string login, QueuedMessage msg)
        {
            var db = Models.DB.ModelContext.Instance;
            var user = db.FindUserByLogin(login);

            db.UserMessages.Add(new DB.DbUserMessage() { UserId = user.Id, Type = (int)msg.type, Content = msg.content });
            db.SaveChanges();
        }

        public static QueuedMessage[] GetUserMessages(string login)
        {
            var db = Models.DB.ModelContext.Instance;
            var user = db.FindUserByLogin(login);

            if (user == null || !db.UserMessages.Any(x => x.UserId == user.Id))
                return new QueuedMessage[] { };
            else
            {
                DB.DbUserMessage[] messages = null;

                lock (msgLock)
                {
                    messages = db.UserMessages.Where(x => x.UserId == user.Id).ToArray();
                    foreach (var msg in messages)
                        db.UserMessages.Remove(msg);
                    db.SaveChanges();
                    // DbUpdateConcurrencyException
                }

                return messages.Select(x => new QueuedMessage() { id = x.Id, content = x.Content, type = (QueuedMessageType)x.Type }).ToArray();
            }
        }
    }
}