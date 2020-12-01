using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMSmax.Data;

namespace RMSmax.Models
{

    public class ArticleRepository : IArticleRepository
    {
        private RMSContext context;
        public ArticleRepository(RMSContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Article> Articles => context.Articles;


    }
}
