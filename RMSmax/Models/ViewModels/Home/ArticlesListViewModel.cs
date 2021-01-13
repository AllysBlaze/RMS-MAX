using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RMSmax.Models.ViewModels
{
    public class ArticlesListViewModel : MainViewModel // aka Home/Index ViewModel
    {
        private string rootPath;
        public IEnumerable<Article> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IList<string> SliderPhotos
        {
            get
            {
                List<string> pics = new List<string>();
                for (int i = 1; i <= 3; i++)// folder 1, 2, 3.
                {
                    string[] files = Directory.GetFiles(Path.Combine(rootPath, "pictures", "picsSlider", i.ToString()));
                    foreach (var v in files)
                    {
                        pics.Add(i.ToString() + "/" + Path.GetFileName(v));
                    }
                }

                return pics;
            }
        }
        public ArticlesListViewModel(IWebHostEnvironment env)
        {
            rootPath = env.WebRootPath;
        }
    }
}
