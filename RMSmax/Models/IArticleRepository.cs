using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RMSmax.Models
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles { get; }
        public void AddArticle(string title, string content, string author, string photoCover, string photoIn);
        public void DeleteArticle(int id);
        public void EditArticle(int id,string title, string content, string author, string photoCover, string photoIn);


    }
}
