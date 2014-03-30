using System.ServiceModel;
using System.ServiceModel.Web;
using MyMvc.Services.DataContracts;

namespace MyMvc.Services
{
    [ServiceContract]
    public interface IAndroidService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetCode(string id);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        UserLocation Connect(string id, string code);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void Disconnect(string id, string other);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void UpdateLocation(string id, double lat, double lng);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        UserLocation[] GetContactLocations(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "process?id={id}")]
        MessageBag Process(string id, QueuedMessage[] messages);
    }
}
