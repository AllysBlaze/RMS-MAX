using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public class FakeEmployeeRepository:IEmployeeRepository
    {
        public IQueryable<Employee> Employees => new List<Employee>
        {
            new Employee{Id=0,Name="Imię0", LastName="Nazwisko0", Degree="Stopień", Department="Katedra", Function="funkcja", 
                Mail="@mail.pl", Phone="number000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=1,Name="Imię1", LastName="Nazwisko1", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="111111111", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=2,Name="Imię2", LastName="Nazwisko2", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=3,Name="Imię3", LastName="Nazwisko3", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=4,Name="Imię4", LastName="Nazwisko4", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=5,Name="Imię5", LastName="Nazwisko5", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=6,Name="Imię6", LastName="Nazwisko6", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=7,Name="Imię7", LastName="Nazwisko7", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=8,Name="Imię8", LastName="Nazwisko8", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=9,Name="Imię9", LastName="Nazwisko9", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=10,Name="Imię10", LastName="Nazwisko10", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=11,Name="Imię11", LastName="Nazwisko11", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=12,Name="Imię12", LastName="Nazwisko12", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=13,Name="Imię13", LastName="Nazwisko13", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=14,Name="Imię14", LastName="Nazwisko14", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=15,Name="Imię15", LastName="Nazwisko15", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},
            new Employee{Id=16,Name="Imię16", LastName="Nazwisko16", Degree="Stopień", Department="Katedra", Function="funkcja",
                Mail="@mail.pl", Phone="000000000", Position="Stanowisko", Room="Pokój", Timetable="link"},

        }.AsQueryable<Employee>();
    }
}
