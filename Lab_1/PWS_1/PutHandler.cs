using System;
using System.Web;
using System.Web.SessionState;
using PWS_1;

namespace PWS_1
{
    public class PutHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            Storage resultStack = (Storage)context.Session["Storage"] ?? new Storage();
            context.Session["Storage"] = resultStack;

            try
            {
                int newElement = int.Parse(req.Params["ADD"]);
                resultStack.Stack.Push(newElement);
                res.Write($"{newElement} добавлено в стэк");
            }
            catch (Exception)
            {
                res.Write("Некорректное значение");
            }
        }
    }
}
