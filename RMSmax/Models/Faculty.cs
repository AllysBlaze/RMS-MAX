using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace RMSmax.Models
{
    public class Faculty
    {
        public static Faculty FacultyInstance { get; private set; }
        private static string configFile = "facultyinfo.json";
        public string Name { get;  set; }
        public string Street { get;  set; }
        public string Postcode { get;  set; }
        public string City { get;  set; }
        public string State { get; set; }
        public string Phone { get;  set; }
        public string Email { get;  set; }
        public string MapSource { get;  set; }
        public IList<Course> Courses { get;  set; }
        public string Color { get; set; }
        public string Logo { get; set; }

        public Faculty() { }
        public Faculty(string webRoot)
        {
            configFile = Path.Combine(webRoot, "config", configFile);
            if (File.Exists(configFile))
            {
                Deserialize();
            }
            else
            {
                Courses = new List<Course>();
            }
            FacultyInstance = this;
        }
        public void Update(Faculty faculty)
        {
            Update(faculty.Name, faculty.Street, faculty.Postcode, faculty.City, faculty.Phone, faculty.Email, faculty.MapSource, faculty.Courses, faculty.Color, faculty.Logo);
        }
        public void Update(string name, string street, string postcode, string city, string phone, string email, string mapSource, IList<Course> courses, string color, string logo)
        {
            Name = name;
            Street = street;
            Postcode = postcode;
            City = city;
            Phone = phone;
            Email = email;
            MapSource = mapSource;
            Courses = courses;
            Color = color;
            Logo = logo;

            Serialize();
        }
        private void Serialize()
        {
            string data = JsonSerializer.Serialize(this);
            File.WriteAllText(configFile, data);
        }
        private void Deserialize()
        {
           try
            {
                string data = File.ReadAllText(configFile);
                Faculty f = JsonSerializer.Deserialize<Faculty>(data);
                this.Name = f.Name;
                this.Street = f.Street;
                this.Postcode = f.Postcode;
                this.City = f.City;
                this.Phone = f.Phone;
                this.Email = f.Email;
                this.MapSource = f.MapSource;
                this.Courses = f.Courses;
                this.Color = f.Color;
                this.Logo = f.Logo;
                this.State = f.State;
            }
            catch (Exception e)
            {
                Courses = new List<Course>();
            }
        }
    }

    public class Course
    {
        public string Name { get; set; }
        public IList<string> Specialties { get; set; }
    }
}
