using System.Collections.Generic;

namespace RMSmax.Models.ViewModels.Admin
{
    public class SubjectsListViewModel : MainViewModel
    {
        public IEnumerable<Subject> Subjects { get; set; }
        public string CourseName { get; set; }
    }
}
