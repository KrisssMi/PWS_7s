using System.Text;
using System.Web.Http;

namespace laba3.Controllers
{
    public class ErrorController : ApiController
    {
        [HttpGet]
        public object Get(int errorCode)
        {
            StringBuilder resultString = new StringBuilder();   // создаем объект StringBuilder для формирования строки
            switch (errorCode)
            {
                case 500:
                    resultString.AppendLine("Internal server error");
                    break;
                case 404:
                    resultString.AppendLine("Not found");
                    break;
                case 400:
                    resultString.AppendLine("You need to use json or xml format");
                    break;
                default:
                    resultString.AppendLine("Unknown error");
                    break;
            }
            return Json(resultString.ToString());   
        }
    }
}
