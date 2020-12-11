using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RMSmax.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public int Degree { get; set; }
        [Required]
        public string File { get; set; }
        [Required]
        public string Course { get; set; }
    }
}
