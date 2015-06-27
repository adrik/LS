using MyMvc.Services.DataContracts.V2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;

namespace MyMvc.Models
{
    public static class Gcm
    {
        private const string SenderId = "GLT Server";

        private static string gcmApiKey = WebConfigurationManager.AppSettings["GcmApiKey"];
        private static JavaScriptSerializer serializer = new JavaScriptSerializer();

        public static void Send(GcmMessage message)
        {
            string json = serializer.Serialize(message);

            WebRequest tRequest = WebRequest.Create("https://android.googleapis.com/gcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/json;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", gcmApiKey));

            tRequest.Headers.Add(string.Format("Sender: id={0}", SenderId));

            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            tRequest.ContentLength = byteArray.Length;

            Stream dataStream = tRequest.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            WebResponse tResponse = tRequest.GetResponse();

            dataStream = tResponse.GetResponseStream();

            StreamReader tReader = new StreamReader(dataStream);

            string sResponseFromServer = tReader.ReadToEnd();
            // TODO: process response

            tReader.Close();
            dataStream.Close();
            tResponse.Close();
        }

        public static void SendData(string receiverToken, ServerMessage message)
        {
            Send(new GcmMessage(receiverToken, message));
        }
    }
}