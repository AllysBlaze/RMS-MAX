using System.Collections.Generic;

namespace RMSmax.Models.ViewModels.Admin
{
    public class ArticleListViewModel : MainViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentSearchingTitle { get; set; }
        public string CurrentSearchingAuthor { get; set; }
    }
}
