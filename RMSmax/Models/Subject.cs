using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public class Subject
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public int Semester { get; set; }
        public int Degree { get; set; }
        public string File { get; set; }
        public string Course { get; set; }
    }
}
