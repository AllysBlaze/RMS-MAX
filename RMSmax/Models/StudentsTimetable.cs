using System.ComponentModel.DataAnnotations;

namespace RMSmax.Models
{
    public class StudentsTimetable
    {
        public int Id { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public string Timetable { get; set; }
        [Required]
        public int Degree { get; set; }
        [Required]
        public string Course { get; set; }


    }
}
