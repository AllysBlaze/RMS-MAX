﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public interface ISubjectRepository
    {
        IQueryable<Subject> Subjects{ get; }
        public void AddSubject(Subject subject);
        public void DeleteSubject(Subject subject);
        public void EditSubject(Subject sub);
    }
}