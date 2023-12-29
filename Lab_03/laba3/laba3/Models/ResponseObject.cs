using laba3.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace laba3.Models
{
    public class ResponseObject 
    {
        public List<StudentApi> Students { get; set; }  // Список студентов
        public List<Link> Links { get; set; }           // Список ссылок
    }
}