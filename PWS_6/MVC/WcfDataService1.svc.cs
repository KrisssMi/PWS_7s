//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
using System.Data.Services;
using System.Data.Services.Common;
using System.Data.Services.Providers;
using System.Linq;
using System.ServiceModel.Web;
using MVC.Edmx;

namespace MVC
{
    public class WcfDataService1 : EntityFrameworkDataService<WSMKVEntities>    // создание службы данных с именем WcfDataService1
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);            // установка прав доступа ко всем сущностям и наборам сущностей
            /*config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);*/
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;  
        }

        [WebGet]                                                                
        public IQueryable<Note> ChangeNote(int id, string subject, int note1, int studentId)    // IQueryable - интерфейс, который представляет набор данных, который может быть вычислен
        {
            var context = CurrentDataSource;
            var note = context.Note.Find(id);
            note.subject = subject;
            note.student_id = studentId;
            note.note1 = note1;
            context.SaveChanges();
            return context.Note;
        }

        [WebGet]
        public IQueryable<Note> InsertNote(string subject, int note1, int studentId)
        {
            var note = new Note
            {
                subject = subject,
                student_id = studentId
            };
            note.note1 = note1;
            var context = CurrentDataSource;
            context.Note.Add(note);
            context.SaveChanges();
            return context.Note;
        }

        [WebGet]
        public IQueryable<Student> ChangeStudent(int id, string name)
        {
            var context = CurrentDataSource;
            var student = context.Student.Find(id);
            student.name = name;
            context.SaveChanges();
            return context.Student;
        }

        [WebGet]
        public IQueryable<Student> InsertStudent(string name)
        {
            var student = new Student
            {
                name = name
            };
            
            var context = CurrentDataSource;
            context.Student.Add(student);
            context.SaveChanges();
            return context.Student;
        }
    }
}
    