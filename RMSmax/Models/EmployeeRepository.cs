using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMSmax.Data;

namespace RMSmax.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private RMSContext context;
        public EmployeeRepository(RMSContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Employee> Employees => context.Employees;
        public void AddEmployee(Employee employee)
        {
            context.AddRange(employee);
            context.SaveChanges();
        }
        public void DeleteEmployee(int id)
        {
            context.Remove(context.Employees.Single(a => a.Id == id));
            context.SaveChanges();
        }
        public void EditEmployee(Employee emp)
        {
            var employee = context.Employees.First(a => a.Id == emp.Id);
            employee.Degree = emp.Degree;
            employee.Department = emp.Department;
            employee.Function = emp.Function;
            employee.LastName = emp.LastName;
            employee.Mail = emp.Mail;
            employee.Name = emp.Name;
            employee.Phone = emp.Phone;
            employee.Photo = emp.Photo;
            employee.Position = emp.Position;
            employee.Room = emp.Room;
            employee.Timetable = emp.Timetable;
            context.SaveChanges();
        }
    }

}
