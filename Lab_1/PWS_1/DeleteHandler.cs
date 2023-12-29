using System.Web;
using System.Web.SessionState;
using PWS_1;

namespace PWS_1
{
    public class DeleteHandler : IHttpHandler, IRequiresSessionState
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {
            var response = context.Response;

            var dataStorage = (Storage)context.Session["Storage"];
            
            if (dataStorage == null)
            {
                dataStorage = new Storage();
                context.Session["Storage"] = dataStorage;
            }

            if (dataStorage.Stack.Count == 0)
            {
                context.Response.StatusCode = 400;
                response.Write("Stack is empty");
            }
            else
            {
                var popedElement = dataStorage.Stack.Pop();

                response.Write($"Deleted element: {popedElement}");
            }
        }
    }
}
