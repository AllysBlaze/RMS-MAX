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
        public IFormFile SliderPhoto1 { set; get; }
        public IFormFile SliderPhoto2 { set; get; }
        public IList<string> SliderPhotos { 
            get 
            {
                List<string> pics = new List<string>();
                for (int i = 1; i <= 3; i++)
                {
                    string[] files = Directory.GetFiles(Path.Combine(rootPath, "pictures", "picsSlider", i.ToString()));
                    foreach (var v in files)
                    {
                        pics.Add(Path.GetFileName(v));
                    }
                    pics.AddRange(files);
                }

                return pics;
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
