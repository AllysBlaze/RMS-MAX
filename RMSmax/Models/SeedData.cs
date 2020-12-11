using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMSmax.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace RMSmax.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            RMSContext context = app.ApplicationServices.GetRequiredService<RMSContext>();
            context.Database.Migrate();
            if (!context.Articles.Any())
            {
                context.Articles.AddRange(
                    new Article
                    {
                        Title = "title1",
                        Content = "content1",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title2",
                        Content = "content2",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title3",
                        Content = "content3",
                        Author = "Author",
                        PhotoIn = "pic5.jpg"
                    },
                    new Article
                    {
                        Title = "title3",
                        Content = "content3",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title3",
                        Content = "content3",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title4",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    }
                    );



            }
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko1",
                        Degree = "Inżynier",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",

                    },
                    new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko2",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",

                    },
                    new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko3",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Matematyki",

                    },
                    new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko4",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",

                    },
                    new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko5",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }
                    );
            }
            if (!context.Subjects.Any())
            {
                context.Subjects.AddRange(
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot1",
                        Semester = 3,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot2",
                        Semester = 1,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot3",
                        Semester = 5,
                    },
                    new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot4",
                        Semester = 1,
                    },
                    new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot5",
                        Semester = 3,
                    },
                    new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot6",
                        Semester = 5,
                    }
                    );
            }
            if (!context.StudentsTimetables.Any())
            {
                context.StudentsTimetables.AddRange(
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        Semester = 1,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        Semester = 3,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        Semester = 5,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 2,
                        Semester = 1,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        Semester = 3,
                        Timetable = "link"

                    }

                    );
            }
            context.SaveChanges();
        }
    }
}
