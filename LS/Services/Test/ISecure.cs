using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace MyMvc.Services.Test
{
    [ServiceContract]
    public interface ISecure
    {
        [OperationContract]
        [WebGet(UriTemplate = "/DoWork")]
        string DoWork();
    }
}
