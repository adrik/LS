using System.ServiceModel;
using System.ServiceModel.Web;
using MyMvc.Services.DataContracts;

namespace MyMvc.Services
{
    [ServiceContract(Namespace = "octo.users.service", Name = "UserService")]
    public interface IAndroidService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "process?id={id}")]
        MessageBag Process(string id, QueuedMessage[] messages);
    }
}
