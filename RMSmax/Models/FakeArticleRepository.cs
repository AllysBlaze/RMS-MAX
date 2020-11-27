using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public class FakeArticleRepository : IArticleRepository
    {
        public IQueryable<Article> Articles => new List<Article> {
            new Article { Title = "Tytuł 1", Content = "Treść" },
            new Article { Title = "Tytuł 2", Content = "Treść" },
            new Article { Title = "Tytuł 3", Content = "Treść" },
            new Article { Title = "Tytuł 4", Content = "Treść" },
            new Article { Title = "Tytuł 5", Content = "Treść" },
            new Article { Title = "Tytuł 6", Content = "Treść" }
        }.AsQueryable<Article>();
    }
}
