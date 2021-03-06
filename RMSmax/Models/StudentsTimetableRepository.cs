﻿using System.Linq;
using RMSmax.Data;

namespace RMSmax.Models
{
    public class StudentsTimetableRepository : IStudentsTimetableRepository
    {
        private RMSContext context;
        public StudentsTimetableRepository(RMSContext ctx)
        {
            context = ctx;
        }
        public IQueryable<StudentsTimetable> StudentsTimetables => context.StudentsTimetables;
        public void AddStudentsTimetable(StudentsTimetable studentsTimetable)
        {
            context.AddRange(studentsTimetable);
            context.SaveChanges();
        }
        public void DeleteStudentsTimetable(int studentsTimetableId)
        {
            context.Remove(context.StudentsTimetables.Single(a => a.Id == studentsTimetableId));
            context.SaveChanges();
        }
        public void EditStudentsTimetable(StudentsTimetable stu)
        {
            var studentsTimetable = context.StudentsTimetables.First(a => a.Id == stu.Id);
            studentsTimetable.Course = stu.Course;
            studentsTimetable.Degree = stu.Degree;
            studentsTimetable.Semester = stu.Semester;
            studentsTimetable.Timetable = stu.Timetable;
            context.SaveChanges();
        }
    }
}
