using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RMSmax.Models.ViewModels
{
    public class ArticlesListViewModel : MainViewModel
    {
        private string rootPath;
        public IEnumerable<Article> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IList<string> SliderPhotos
        {
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
        public ArticlesListViewModel() { }
        public ArticlesListViewModel(IWebHostEnvironment env)
        {
            rootPath = env.WebRootPath;
        }
    }
}
