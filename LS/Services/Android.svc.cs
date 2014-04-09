using System;
using System.Linq;
using System.ServiceModel.Activation;
using MyMvc.Models;
using MyMvc.Services.DataContracts;
using MyMvc.Models.MessageProcessing;

namespace MyMvc.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Android : IAndroidService
    {
        public string GetCode(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException();

            string code = CodeGen.Next();
            UserFunctions.CreateUpdateUserCode(GetLogin(id), code);

            return code;
        }

        public UserLocation Connect(string id, string code)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException();

            string login = GetLogin(id);

            int test;

            if (int.TryParse(code, out test) && test > 0 && test < 7)
            {
                code = UserFunctions.SelectUser(test, x => x.User.Code).FirstOrDefault();
            }

            string contactLogin;

            if (UserFunctions.Connect(login, code, out contactLogin))
                MsgProcessor.SaveMessageForUser(contactLogin, new QueuedMessage() { Content = FormatUserInfo(login), Type = QueuedMessageType.RequestClientConnect });

            if (contactLogin == null)
                return null;
            else
                return 
                    UserFunctions.SelectUser(
                        contactLogin,
                        x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng }).FirstOrDefault();
        }

        public void Disconnect(string id, string other)
        {
            string login = GetLogin(id);

            if (UserFunctions.Disconnect(login, other))
                MsgProcessor.SaveMessageForUser(other, new QueuedMessage() { Content = login, Type = QueuedMessageType.RequestClientDisconnect });
        }

        public void UpdateLocation(string id, double lat, double lng)
        {
            UserFunctions.UpdateLocation(GetLogin(id), lat, lng);
        }

        public UserLocation[] GetContactLocations(string id)
        {
            return 
                UserFunctions.SelectContacts(
                    GetLogin(id),
                    x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng }).ToArray();
        }

        public MessageBag Process(string id, QueuedMessage[] messages)
        {
            string login = GetLogin(id);

            MessageResponse[] answers = MsgProcessor.Process(login, messages);
            QueuedMessage[] storedMessages = MsgProcessor.GetUserMessages(login);

            return new MessageBag() { Answers = answers, Messages = storedMessages };
        }

        private string GetLogin(string id)
        {
            return id;
        }

        private string FormatUserInfo(string login)
        {
            var details =
                    UserFunctions.SelectUser(
                        login,
                        x => new UserLocation { id = x.User.Login, lat = x.Location.Lat, lng = x.Location.Lng }).FirstOrDefault();

            return string.Format("{0}|{1}|{2}", details.id, details.lat, details.lng);
        }
    }
}
