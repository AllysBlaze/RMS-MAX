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
    }
}
