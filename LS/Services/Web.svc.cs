using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Serialization;
using WebMatrix.WebData;
using System.Web.Helpers;
using MyMvc.Models;
using MyMvc.Services.DataContracts;

namespace MyMvc.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Web
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public DeviceInfo[] GetUserInfo()
        {
            return 
                UserFunctions.SelectUser(
                    WebSecurity.CurrentUserId,
                    x => new DeviceInfo { uid = x.User.Id, uname = x.User.Login, dname = x.Device.Name, id = x.Device.Id, lat = x.Location.Lat, lng = x.Location.Lng }).ToArray();
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public DeviceInfo[] GetContactsInfo()
        {
            return 
                UserFunctions.SelectContacts(
                    WebSecurity.CurrentUserId,
                    x => new DeviceInfo { uid = x.User.Id, uname = x.User.Login, dname = x.Device.Name, id = x.Device.Id, lat = x.Location.Lat, lng = x.Location.Lng }).ToArray();
        }

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public DeviceLocation[] GetAllLocations()
        {
            return 
                UserFunctions.SelectAll(
                    WebSecurity.CurrentUserId, 
                    x => new DeviceLocation { id = x.Device.Id, lat = x.Location.Lat, lng = x.Location.Lng }).ToArray();
        }


        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public void Randomize()
        {
            var db = Models.DB.ModelContext.Instance;
            Random r = new Random();

            foreach (var user in db.Users.ToList())
            {
                var loc = UserFunctions.SelectUser(user.Id, x => x.Location).FirstOrDefault();

                double dlat = (r.NextDouble() - 0.5) / 10;
                double dlng = (r.NextDouble() - 0.5) / 10;

                UserFunctions.UpdateLocation(user.Id, loc.Lat + dlat, loc.Lng + dlng);
            }
        }
    }
}
