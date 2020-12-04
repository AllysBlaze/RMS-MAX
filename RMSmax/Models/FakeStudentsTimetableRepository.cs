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
            new StudentsTimetable{Id=0,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=1,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=2,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=3,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=4,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=5,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=6,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=7,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=8,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=8,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            new StudentsTimetable{Id=9,Semester="Semestr",Degree="Stopień", Timetable="Link do planu",Course="Kierunek"},
            }.AsQueryable<StudentsTimetable>();
    }
}
