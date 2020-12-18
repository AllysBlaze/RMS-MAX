using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMSmax.Data;

namespace RMSmax.Models
{
    public class SubjectRepository:ISubjectRepository
    {
        private RMSContext context;
        public SubjectRepository(RMSContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Subject> Subjects => context.Subjects;
        public void AddSubject(Subject subject)
        {
            context.AddRange(subject);
            context.SaveChanges();
        }
        public void DeleteSubject(int subjectId)
        {
            context.Remove(context.Subjects.Single(a => a.Id == subjectId));
            context.SaveChanges();
        }
        public void EditSubject(Subject sub)
        {
            var subject = context.Subjects.First(a => a.Id == sub.Id);
            subject.Course = sub.Course;
            subject.Degree = sub.Degree;
            subject.File = sub.File;
            subject.Name = sub.Name;
            subject.Semester = sub.Semester;
            context.SaveChanges();
        }
    }
}
