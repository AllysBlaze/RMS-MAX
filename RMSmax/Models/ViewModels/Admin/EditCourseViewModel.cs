using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EditCourseViewModel : MainViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<StudentsTimetable> StudentsTimetables { get; set; }
        public IFormFileCollection StudyPlans { get; set; }
    }
}
