﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public class StudentsTimetable
    {
        public uint Id { get; set; }
        public int Semester { get; set; }
        public string Timetable { get; set; }
        public int Degree { get; set; }
        public string Course { get; set; }


    }
}
