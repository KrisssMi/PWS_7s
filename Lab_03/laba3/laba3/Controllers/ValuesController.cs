using laba3.Models;
using System.Web.Http;
using System.Web.Http.Description;
using laba3.Services;
using System.Net;
using System;
using Newtonsoft.Json;

namespace laba3.Controllers
{
    public class ValuesController : ApiController
    {
        private static readonly StudentService studentService = new StudentService();   // создаем объект класса StudentService

        [HttpGet]
        public object Get(string sort = "id", string limit = "10", string offset = "0", string minid = "0", string maxid = "10000", 
            string like = null, string globallike = null, string columns = "id, name, phone")
        {
            var type = Request.Headers.Accept.ToString() ?? "json";
            var data = studentService.GetStudents(type, sort, Convert.ToInt32(limit), Convert.ToInt32(offset), Convert.ToInt32(minid),
                Convert.ToInt32(maxid), like, globallike, columns);
            if (data.GetType().Name == "Link")  // если возвращается объект типа Link, то это означает, что была допущена ошибка
                return Content(HttpStatusCode.BadRequest, data);    
            else
                if (type != "json")
                return Content(HttpStatusCode.OK, data);    // возвращаем xml
            else
                return Json(data);
        }

        public object Get(int id)   
        {
            var type = Request.Headers.Accept.ToString();   // определяем тип данных, который хочет получить клиент
            var data = studentService.GetStudent(id, type); 
            if (data.GetType().Name == "Link")  
                return Content(HttpStatusCode.BadRequest, data);
            else
                return Content(HttpStatusCode.OK, data);
        }

        [HttpPost]
        [ResponseType(typeof(Student))]
        public object Post(Student student)
        {
            studentService.Add(student);
            return Content(HttpStatusCode.Created, "Student created");
        }

        [HttpPut]
        [ResponseType(typeof(Student))]
        public object Put(Student student)
        {
            studentService.Update(student);
            return Content(HttpStatusCode.OK, "Student modified");
        }

        [HttpDelete]
        public object Delete(int id)
        {
            studentService.RemoveById(id);
            return Content(HttpStatusCode.OK, "Student removed");
        }
    }
}