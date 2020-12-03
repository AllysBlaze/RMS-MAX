using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public class Employee
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Degree { get; set; }
        public string Department { get; set; }//Katedra
        public string Function { get; set; }
        public string Position { get; set; } //stanowisko
        public string Room { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Timetable { get; set; }
    }
}
