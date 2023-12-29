using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSMKVModel;

namespace PWS_6_client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                int choise = 1;
                while (choise != 0)
                {
                    Console.WriteLine("1.Add new row\n" +
                                        "2.Update student name\n" +
                                        "3.Print all rows\n" +
                                        "0.Exit");
                    choise = int.Parse(Console.ReadLine());
                    switch (choise)
                    {
                        case 1: Add(); break;
                        case 2: Update(); break;
                        case 3: PrintValues(); break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }

        static void PrintValues()   
        {
            WSMKVEntities service = new WSMKVEntities(new Uri("http://localhost:49240/WcfDataService1.svc"));   // вызываем сервис

            foreach (var student in service.Student.AsEnumerable())
            {
                Console.WriteLine("\t----------------------\t");
                Console.WriteLine($"Id: {student.id}\nName: {student.name}");
                Console.WriteLine();
                foreach (var note in service.Note.Where(i => i.student_id == student.id).AsEnumerable())        // выводим все оценки студента
                {
                    Console.WriteLine($"  Subj: {note.subject}, Note: {note.note1}");
                }
                Console.WriteLine("\t----------------------\t");
            }
            Console.WriteLine("\n\n\n");
        }
        static void Add()
        {
            WSMKVEntities service = new WSMKVEntities(new Uri("http://localhost:49240/WcfDataService1.svc"));

            Student student = new Student() { id = 100 };   
            Console.Write("Enter Name: ");
            student.name = Console.ReadLine();
            service.AddToStudent(student);
            service.SaveChanges();
        }
        static void Update()
        {
            WSMKVEntities service = new WSMKVEntities(new Uri("http://localhost:49240/WcfDataService1.svc"));
            Console.Write("Enter Id: ");
            int id = int.Parse(Console.ReadLine());
            var student = service.Student.AsEnumerable().First(i => i.id == id);    // находим студента по id
            Console.Write("New Name: ");
            student.name = Console.ReadLine();
            service.UpdateObject(student);
            service.SaveChanges();
        }
    }
}