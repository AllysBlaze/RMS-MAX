using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models.ViewModels.Admin
{
    public class ArticleListViewModel : MainViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentSearchingTitle { get; set; }
        public string CurrentSearchingAuthor { get; set; }
        public DateTime CurrentSearchingFrom { get; set; }
        public DateTime CurrentSearchingTo { get; set; }
    }
}
