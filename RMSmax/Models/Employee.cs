using System.ComponentModel.DataAnnotations;

namespace RMSmax.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Degree { get; set; }
        public string Department { get; set; }
        public string Function { get; set; }
        public string Position { get; set; }
        public string Room { get; set; }
        [Required]
        public string Mail { get; set; }
        [StringLength(9)]
        public string Phone { get; set; }
        public string Timetable { get; set; }
        public string Photo { get; set; }
    }
}
