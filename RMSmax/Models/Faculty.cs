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
                Deserialize(configFile);
            }
            else
            {
                string path = Path.Combine(webRoot, "config", "defaultFacultyinfo.json");
                Deserialize(path);
                Courses = new List<Course>();
            }
            FacultyInstance = this;
        }

        public void Serialize()
        {
            string data = JsonSerializer.Serialize(FacultyInstance);
            File.WriteAllText(configFile, data);
        }
        private void Deserialize(string path)
        {
           try
            {
                string data = File.ReadAllText(path);
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
