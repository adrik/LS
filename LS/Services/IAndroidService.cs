using System.ServiceModel;
using System.ServiceModel.Web;
using MyMvc.Services.DataContracts;
using MyMvc.Services.DataContracts.V2;
using System;

namespace MyMvc.Services
{
    [ServiceContract]
    public interface IAndroidService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, 
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/process")]
        ResponseBag Process(RequestBag request);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/query")]
        ResponseData Query(RequestData request);

        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/exchange")]
        ExchangeResponse Exchange(ExchangeRequest request);
    }
}
