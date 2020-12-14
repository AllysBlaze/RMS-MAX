using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public interface IStudentsTimetableRepository
    {
        IQueryable<StudentsTimetable> StudentsTimetables { get; }
        public void AddStudentsTimetable(StudentsTimetable studentsTimetable);
        public void DeleteStudentsTimetable(StudentsTimetable studentsTimetable);
        public void EditStudentsTimetable(StudentsTimetable stu);
    }
}
