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

    }
}
