using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public class FakeSubjectRepository:ISubjectRepository
    {
        public IQueryable<Subject> Subjects => new List<Subject>
        {
            new Subject{Id=0,Degree=1, Name="Nazwa", File="Plik" ,Semester=1, Course="Matematyka"},
            new Subject{Id=1,Degree=2, Name="Nazwa", File="Plik" ,Semester=1,Course="Matematyka"},
            new Subject{Id=2,Degree=2, Name="Nazwa", File="Plik" ,Semester=2,Course="Matematyka"},
            new Subject{Id=3,Degree=1, Name="Nazwa", File="Plik" ,Semester=1,Course="Informatyka"},
            new Subject{Id=4,Degree=2, Name="Nazwa", File="Plik" ,Semester=1,Course="Informatyka"},
            new Subject{Id=5,Degree=1, Name="Nazwa", File="Plik" ,Semester=2,Course="Informatyka"},
            new Subject{Id=6,Degree=1, Name="Nazwa", File="Plik" ,Semester=2,Course="Informatyka"},
            new Subject{Id=7,Degree=1, Name="Nazwa", File="Plik" ,Semester=2,Course="Informatyka"},
            new Subject{Id=8,Degree=1, Name="Nazwa", File="Plik" ,Semester=2,Course="Informatyka"},
            new Subject{Id=9,Degree=1, Name="Nazwa", File="Plik" ,Semester=2,Course="Informatyka"},
            new Subject{Id=10,Degree=1, Name="Nazwa", File="Plik",Semester=3,Course="Informatyka"}

        }.AsQueryable<Subject>();

    }
}
