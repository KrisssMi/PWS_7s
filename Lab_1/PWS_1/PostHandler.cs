using System;
using System.IO;
using System.Web;
using System.Web.SessionState;
using PWS_1;

namespace PWS_1
{
    public class PostHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            HttpResponse res = context.Response;
            Storage resultStack = (Storage)context.Session["Storage"] ?? new Storage();
            context.Session["Storage"] = resultStack;

            try
            {
                resultStack.Result = int.Parse(req.Params["RESULT"]);
                res.Write("Result = " + resultStack.Result);
            }
            catch (Exception)
            {
                res.Write("Некорректное значение");
            }
        }
    }
}