using System;
using System.Web;

namespace MyMvc.Handlers
{
    public class ThumbnailHandler : IHttpHandler
    {
        /// <summary>
        /// Вам потребуется настроить этот обработчик в файле web.config вашего 
        /// веб-узла и зарегистрировать его с помощью IIS, чтобы затем воспользоваться им.
        /// Дополнительные сведения см. по ссылке: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string file = System.IO.Path.GetFileNameWithoutExtension(context.Request.FilePath);
            int a = 1;
            context.Response.Write("hahaha");
            context.Response.End();
        }

        #endregion
    }
}
