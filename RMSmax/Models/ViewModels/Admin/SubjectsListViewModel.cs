using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models.ViewModels.Admin
{
    public class SubjectsListViewModel : MainViewModel
    {
        public IEnumerable<Subject> Subjects { get; set; }
    }
}
