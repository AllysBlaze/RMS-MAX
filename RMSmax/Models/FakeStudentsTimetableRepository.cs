using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public class FakeStudentsTimetableRepository :IStudentsTimetableRepository
    {
        public IQueryable<StudentsTimetable> StudentsTimetables => new List<StudentsTimetable>
        {
            new StudentsTimetable{Id=0,Semester="1",Degree="1", Timetable="Link do planu",Course="Informatyka"},
            new StudentsTimetable{Id=1,Semester="2",Degree="1", Timetable="Link do planu",Course="Informatyka"},
            new StudentsTimetable{Id=2,Semester="1",Degree="2", Timetable="Link do planu",Course="Informatyka"},
            new StudentsTimetable{Id=3,Semester="1",Degree="1", Timetable="Link do planu",Course="Matematyka"},
            new StudentsTimetable{Id=4,Semester="2",Degree="1", Timetable="Link do planu",Course="Matematyka"},
            new StudentsTimetable{Id=5,Semester="1",Degree="2", Timetable="Link do planu",Course="Matematyka"},
            new StudentsTimetable{Id=6,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=7,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=8,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=8,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=9,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            }.AsQueryable<StudentsTimetable>();
    }
}
