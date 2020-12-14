using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles { get; }
        public void AddArticle(Article article);
        public void DeleteArticle(Article article);
        public void EditArticle(Article art);


    }
}
