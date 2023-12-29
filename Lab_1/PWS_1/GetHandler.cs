using System.Web;
using System.Web.SessionState;
using PWS_1;

namespace PWS_1
{
    public class GetHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable => false;    // один экземпляр обработчика для обработки всех запросов

        public void ProcessRequest(HttpContext context) 
        {
            var storage = (Storage)context.Session["Storage"];
            if (storage == null)    
            {
                storage = new Storage();
                context.Session["Storage"] = storage;
            }

            context.Response.Write(
                storage.Result +
                (storage.Stack.Count == 0 ? 0 : storage.Stack.Peek())
            );
        }
    }
}