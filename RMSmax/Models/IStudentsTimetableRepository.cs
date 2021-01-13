using System.Linq;

namespace RMSmax.Models
{
    public interface IStudentsTimetableRepository
    {
        IQueryable<StudentsTimetable> StudentsTimetables { get; }
        public void AddStudentsTimetable(StudentsTimetable studentsTimetable);
        public void DeleteStudentsTimetable(int studentsTimetableId);
        public void EditStudentsTimetable(StudentsTimetable stu);
    }
}
