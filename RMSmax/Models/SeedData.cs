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
                        Title = "Testowy obrazek",
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
                    },
                    new Article
                    {
                        Title = "title5",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title6",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title7",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title8",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title9",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title10",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title11",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title12",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title13",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic5.jpg",
                        PhotoCover = "pic5.jpg",
                    },
                    new Article
                    {
                        Title = "title14",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic4.jpg",
                        PhotoCover = "pic4.jpg",
                    },
                    new Article
                    {
                        Title = "title15",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic1.jpg",
                        PhotoCover = "pic1.jpg",
                    },
                    new Article
                    {
                        Title = "title16",
                        Content = "content4",
                        Author = "Author",
                        PhotoIn = "pic7.jpg",
                        PhotoCover = "pic7.jpg",
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

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko6",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko7",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko8",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko9",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko10",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko11",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko12",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko13",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    },
                    new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko13",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko14",
                        Degree = "Doktor",
                        Mail = "imie.nazwisko@polsl.pl",
                        Phone = "000000000",
                        Photo = "pic5.jpg",
                        Department = "Katedra Informatyki",
                        Function = "Prodziekan",
                        Position = "Asystent",
                        Room = "MS 505",
                        Timetable = "link",

                    }, new Employee
                    {
                        Name = "Imię1",
                        LastName = "Nazwisko15",
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
                        Semester = 1,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot2",
                        Semester = 2,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot3",
                        Semester = 3,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot3",
                        Semester = 4,
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
                        Course = "Informatyka",
                        Degree = 2,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot1",
                        Semester = 1,
                    },
                    new Subject
                    {
                        Course = "Informatyka",
                        Degree = 2,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot2",
                        Semester = 2,
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
                        Semester = 2,
                    },
                    new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot6",
                        Semester = 3,
                    }, new Subject
                    {
                        Course = "Matematyka",
                        Degree = 1,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot6",
                        Semester = 4,
                    }, new Subject
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot6",
                        Semester = 1,
                    }, new Subject
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        File = "inf_I_2019.pdf",
                        Name = "Przedmiot6",
                        Semester = 2,
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
                        Semester = 4,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        Semester = 2,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Matematyka",
                        Degree = 2,
                        Semester = 3,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 1,
                        Timetable = "link"
                    },
                    new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 2,
                        Timetable = "link"

                    }, new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 3,
                        Timetable = "link"

                    }, new StudentsTimetable
                    {
                        Course = "Informatyka",
                        Degree = 1,
                        Semester = 4,
                        Timetable = "link"

                    },
                     new StudentsTimetable
                     {
                         Course = "Informatyka",
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
                        Course = "Informatyka",
                        Degree = 2,
                        Semester = 2,
                        Timetable = "link"

                    }

                    );
            }
            context.SaveChanges();
        }
    }
}
