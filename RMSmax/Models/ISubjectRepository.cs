using System.Linq;

namespace RMSmax.Models
{
    public interface ISubjectRepository
    {
        IQueryable<Subject> Subjects{ get; }
        public void AddSubject(Subject subject);
        public void DeleteSubject(int subjectId);
        public void EditSubject(Subject sub);
    }
}
