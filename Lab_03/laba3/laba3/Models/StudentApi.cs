using laba3.Models;
using System.Collections;
using System.Collections.Generic;

namespace laba3.Controllers
{
    public class StudentApi
    {
        public int? Id { get ; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public List<Link> Links { get; set; }   

        public StudentApi(Student stud, List<Link> links)
        {
            Id = stud.id;
            Name = stud.name;
            Phone = stud.phone;
            this.Links = links;
        }

        public StudentApi()
        {

        }
    }
}