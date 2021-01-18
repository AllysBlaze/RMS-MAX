using System;
using Xunit;
using RMSmax.Models;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace RMSmax.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void FacultyInfoSerialization()
        {
            string name = "RMSmax";
            string street = "Ulica";
            string postcode = "40-100";
            string state = "Woj";
            string city = "Gliwice";
            string phone = "111222333";
            string color = "#000000";
            string email = "rms@polsl.pl";
            List<Course> courses = new List<Course> { new Course("K1") { FirstDegreeSpecialties = new string[] { "S1", "S2" } }, new Course("K2") };

            string path = "D:\\Programowanie\\ASP.NET\\RMSmax\\RMSmax.Tests\\Files";
            Faculty faculty = new Faculty(path, "facultyinfo.json");
            faculty.Name = name;
            faculty.Street = street;
            faculty.Postcode = postcode;
            faculty.City = city;
            faculty.Color = color;
            faculty.Email = email;
            faculty.Logo = null;
            faculty.Courses = courses;
            faculty.State = state;
            faculty.Phone = phone;
            faculty.MapSource = "";

            faculty.Serialize();

            string data = File.ReadAllText(Path.Combine(path, "config", "facultyinfo.json"));
            Faculty f = JsonSerializer.Deserialize<Faculty>(data);

            Assert.Equal(f.Name, name);
            Assert.Equal(f.Street, street);
            Assert.Equal(f.Postcode, postcode);
            Assert.Equal(f.State, state);
            Assert.Equal(f.City, city);
            Assert.Equal(f.Phone, phone);
            Assert.Equal(f.Color, color);
            Assert.Equal(f.Email, email);
            for (int i = 0; i < courses.Count; i++)
            {
                Assert.Equal(f.Courses.ElementAt(i).Name, courses[i].Name);
                for (int j = 0; j < courses[i].FirstDegreeSpecialties.Count(); i++)
                {
                    Assert.Equal(f.Courses.ElementAt(i).FirstDegreeSpecialties[j], courses[i].FirstDegreeSpecialties[j]);
                }
                for (int j = 0; j < courses[i].SecondDegreeSpecialties.Count(); i++)
                {
                    Assert.Equal(f.Courses.ElementAt(i).SecondDegreeSpecialties[j], courses[i].SecondDegreeSpecialties[j]);
                }
            }
        }
    }
}
