using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMSmax.Data;

namespace RMSmax.Models
{
    public class FakeArticleRepository : IArticleRepository
    {
        public IQueryable<Article> Articles => new List<Article> {
            new Article { Id = 0, Title = "Tytuł 1", Content = "Treść" },
            new Article { Id = 1, Title = "Tytuł 2", Content = "Treść" },
            new Article { Id = 2, Title = "Tytuł 3", Content = "Treść" },
            new Article { Id = 3, Title = "Tytuł 4", Content = "Treść" },
            new Article { Id = 4, Title = "Tytuł 5", Content = "Treść" },
            new Article { Id = 5, Title = "Tytuł 6", Content = "Treść" },
            new Article { Id = 6, Title = "Tytuł 7", Content = "Treść" },
            new Article { Id = 7, Title = "Tytuł 8", Content = "Treść" },
            new Article { Id = 8, Title = "Tytuł 9", Content = "Treść" },
            new Article { Id = 9, Title = "Tytuł 10", Content = "Treść" },
            new Article { Id = 10, Title = "Tytuł 11", Content = "Treść" },
            new Article { Id = 11, Title = "Tytuł 12", Content = "Treść" },
            new Article { Id = 12, Title = "Tytuł 13", Content = "Treść" }
        }.AsQueryable<Article>();
        

    }
}
