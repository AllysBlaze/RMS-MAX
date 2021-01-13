using System.Linq;

namespace RMSmax.Models
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> Employees{ get; }
        public void AddEmployee(Employee employee);
        public void DeleteEmployee(int id);
        public void EditEmployee(Employee emp);
    }
}
