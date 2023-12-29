using laba3.Controllers;
using laba3.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace laba3.Services
{
    public class StudentService
    {
        private static readonly Context Context = new Context();        // Создаем контекст данных

        private readonly DbSet<Student> students = Context.Students;    // Создаем объект для работы с таблицей Students

        public List<Student> GetAll() => students.ToList();     

        public object GetStudents(string type, string sort, int limit, int offset, int minid, int maxid,string like, string globallike, string columns) 
        {
            if (type == null)
                type = "json";
            if (sort == null)
                sort = "id";
            if (limit == 0)
                limit = 2;
            if (maxid == 0)
                maxid = 100;
            if (columns == null)
                columns = "id, name, phone";

            List<Student> students = Context.Students.ToList();  

            if (sort == "name")
                students = students.OrderBy(prop => prop.name).ToList();
            else if (sort == "id")
                students = students.OrderBy(prop => prop.id).ToList();
            else if (sort == "phone")
                students = students.OrderBy(prop => prop.phone).ToList();
            else
                return JsonConvert.SerializeObject(new Link("Use for parameter sort values name and id", "/api/Error?errorCode=400", "GET"));

            int totalPages = (int)Math.Ceiling((double)students.Count / limit);

            // Создаем ссылки на предыдущую и следующую страницы
            var paginationLinks = new List<Link>();
            
            if (offset > 0)
            {
                int prevOffset = Math.Max(0, offset - limit);
                var prevLink = new Link("prev", $"/api/Values?type={type}&sort={sort}&limit={limit}&offset={prevOffset}&minid={minid}&maxid={maxid}&like={like}&globallike={globallike}&columns={columns}", "GET");
                paginationLinks.Add(prevLink);
            }
            if (offset + limit < students.Count)    // если текущая страница не последняя
            {
                int nextOffset = offset + limit;
                var nextLink = new Link("next", $"/api/Values?type={type}&sort={sort}&limit={limit}&offset={nextOffset}&minid={minid}&maxid={maxid}&like={like}&globallike={globallike}&columns={columns}", "GET");
                paginationLinks.Add(nextLink);
            }

            students = students.Skip(offset).Take(limit).Where(prop => prop.id >= minid && prop.id <= maxid).ToList();  // выбираем из списка студентов только те, id которых входят в диапазон minid и maxid

            if (like != null)
            {
                students = students.Where(prop => prop.name.ToLower().Contains(like.ToLower())).ToList();   // выбираем из списка студентов только те, name которых содержит like
            }

            if (globallike != null)
            {
                students = students.Where(prop => (prop.id + prop.name + prop.phone).ToLower().Contains(globallike.ToLower())).ToList();    // выбираем из списка студентов только те, id, name или phone которых содержит globallike
            }

            
            // ----------------- OUTPUT: -----------------

            List<StudentApi> studentsApi = new List<StudentApi>();  // создаем список студентов для вывода
            string[] columsArr = columns.Split(',');                // разбиваем строку columns на массив строк по разделителю ','
            foreach (Student student in students)   
            {
                StudentApi studentApi = new StudentApi();           // создаем объект для вывода

                for (int i = 0; i < columsArr.Length; i++)
                    if (columsArr[i].Contains("id"))
                    {
                        studentApi.Id = student.id;
                    } 
                for (int i = 0; i < columsArr.Length; i++)
                    if (columsArr[i].Contains("name"))
                    {
                        studentApi.Name = student.name;
                    }

                for (int i = 0; i < columsArr.Length; i++)
                    if (columsArr[i].Contains("phone"))
                    {
                        studentApi.Phone = student.phone;
                    }
                var links = new List<Link>()    // создаем список ссылок для студента
                {
                    new Link("self", "/api/Values/" + student.id, "GET"),
                };
                studentApi.Links = links;       // добавляем в объект студента список ссылок
                studentsApi.Add(studentApi);    // добавляем в список студентов студента с ссылками
            }

            ResponseObject responseObject = new ResponseObject()  // создаем объект, который будет возвращаться в ответ на запрос
            {
                Links = new List<Link>()
                {
                    new Link("Create", "/api/Values/" , "POST"),
                },
                Students = studentsApi  
            };
            responseObject.Links.AddRange(paginationLinks);     
            
            if (type.ToLower() == "xml")
            {
                using (StringWriter stringwriter = new System.IO.StringWriter())    // сериализуем объект в xml
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();     // убираем из xml декларацию версии и кодировки
                    ns.Add("", "");     
                    using (XmlWriter writer = XmlWriter.Create(stringwriter, new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false }))
                    {
                        var serializer = new XmlSerializer(responseObject.GetType());
                        serializer.Serialize(writer, responseObject, ns);           // сериализуем объект в xml
                    }
                    return stringwriter.ToString();
                }
            }
            else if (type.ToLower() == "json")
            return JsonConvert.SerializeObject(responseObject,
                    Newtonsoft.Json.Formatting.None,    
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore                // убираем из json пустые поля
                    });
            else
                return JsonConvert.SerializeObject(new Link("Use for parameter type values json and xml", "/api/Error?errorCode=400", "GET"));
        }

        public object GetStudent(int id, string type)
        {
            StudentService studentService = new StudentService();   // создаем объект для работы с БД
            Student student = studentService.GetById(id);

            if (student != null)
            {
                var links = new List<Link>()    
                {
                    new Link("self", "/api/Values/" + student.id, "PUT"),
                    new Link("self", "/api/Values/" + student.id, "DELETE")
                };

                if (type.ToLower() == "xml")
                {
                    using (StringWriter stringwriter = new System.IO.StringWriter())
                    {
                        StudentApi studentApi = new StudentApi(student, links);
                        var serializer = new XmlSerializer(studentApi.GetType());
                        serializer.Serialize(stringwriter, studentApi);
                        return stringwriter.ToString();
                    }
                }
                else if (type.ToLower() == "json")
                    return JsonConvert.SerializeObject(new StudentApi(student, links));
                else
                    return JsonConvert.SerializeObject(new Link("Pls use for parameter type values json and xml", "/api/Error?errorCode=400", "GET"));
            }
            else
                return JsonConvert.SerializeObject(new Link("Student with id " + id + " not exist", "/api/Error?errorCode=404", "GET"));
        }

        public void Add(Student student)
        {
            if (student == null)
            {
                // Обработка случая, когда объект Student равен null
                string errorMessage = "Объект Student не может быть null.";
                // Внедряем JavaScript-код для вывода сообщения в alert
                System.Web.HttpContext.Current.Response.Write($"<script>alert('{errorMessage}');</script>");
                // Завершаем выполнение метода
                return;
            }

            if (string.IsNullOrWhiteSpace(student.name) || string.IsNullOrWhiteSpace(student.phone))
            {
                // Обработка случая, когда Name или Phone пусты или состоят из пробелов
                string errorMessage = "Name и Phone не могут быть пустыми или состоять только из пробелов.";
                // Внедряем JavaScript-код для вывода сообщения в alert
                System.Web.HttpContext.Current.Response.Write($"<script>alert('{errorMessage}');</script>");
                // Завершаем выполнение метода
                return;
            }

            // Добавляем студента и сохраняем изменения в базе данных
            students.Add(student);
            Context.SaveChanges();
        }


        public void Update(Student student)
        {
            var stud = GetById(student.id);
            stud.name = student.name;
            stud.phone = student.phone;
            Context.SaveChanges();
        }

        public void RemoveById(int id)
        {
            var stud = GetById(id);
            students.Remove(stud);
            Context.SaveChanges();
        }

        public Student GetById(int id) => GetAll().Find(stud => stud.id == id);
    }
}