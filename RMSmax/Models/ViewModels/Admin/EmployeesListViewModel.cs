using System.Collections.Generic;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EmployeesListViewModel : MainViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentSearchingName { get; set; }
        public string CurrentSearchingSurname { get; set; }
    }
}
