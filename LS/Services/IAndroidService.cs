using System.ServiceModel;
using System.ServiceModel.Web;
using MyMvc.Services.DataContracts;

namespace MyMvc.Services
{
    [ServiceContract]
    public interface IAndroidService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/process")]
        ResponseBag Process(RequestBag request);
    }
}
