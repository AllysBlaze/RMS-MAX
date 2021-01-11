using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RMSmax.Models.ViewModels.Admin
{
    public class IndexViewModel : MainViewModel
    {
        public IFormFile LogoFile { set; get; }
        public IFormFile SliderPhoto{ set; get; }
        public string NewCourseName { get; set; }
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }
        
    }
}
