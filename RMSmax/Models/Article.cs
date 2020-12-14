using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RMSmax.Models
{
    public class Article
    {
        public int Id { get; set; } //SQL nie radzi sobie z uInt
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateTime{ get; set; } = DateTime.Now;
        [Required]
        public string Author { get; set; }
        public string PhotoIn { get; set; }
        public string PhotoCover { get; set; }
    }
}
