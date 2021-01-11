using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RMSmax.Models.ViewModels.Admin
{
    public class IndexViewModel : MainViewModel
    {
        private string rootPath;
        public IFormFile LogoFile { set; get; }
        public IFormFile SliderPhoto{ set; get; }
        public string SliderPic1 { 
            get 
            {
                string[] files = Directory.GetFiles(Path.Combine(rootPath, "pictures", "picsSlider", "1"));
                return files.Length > 0 ? Path.GetFileName(files[0]) : null;
            } 
        }
        public string SliderPic2 { 
            get 
            {
                string[] files = Directory.GetFiles(Path.Combine(rootPath, "pictures", "picsSlider", "2"));
                return files.Length > 0 ? Path.GetFileName(files[0]) : null;
            } 
        }
        public string SliderPic3 { 
            get 
            {
                string[] files = Directory.GetFiles(Path.Combine(rootPath, "pictures", "picsSlider", "3"));
                return files.Length > 0 ? Path.GetFileName(files[0]) : null;
            } 
        }
        public string NewCourseName { get; set; }
        public string Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }

        public IndexViewModel() { }
        public IndexViewModel(IWebHostEnvironment env)
        {
            rootPath = env.WebRootPath;
        }
    }
}
