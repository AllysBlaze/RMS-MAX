﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    interface IStudentsTimetableRepository
    {
        IQueryable<StudentsTimetable> StudentsTimetables { get; }
    }
}
