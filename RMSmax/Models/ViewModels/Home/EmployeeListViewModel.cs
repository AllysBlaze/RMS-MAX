using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models.ViewModels
{
    public class EmployeeListViewModel : MainViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentSearchingName { get; set; }
        public string CurrentSearchingSurname { get; set; }
    }
}
