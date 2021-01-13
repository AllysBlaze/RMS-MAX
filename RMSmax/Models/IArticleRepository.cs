using System.Linq;

namespace RMSmax.Models
{
    public interface IArticleRepository
    {
        IQueryable<Article> Articles { get; }
        public void AddArticle(Article article);
        public void DeleteArticle(int id);
        public void EditArticle(Article art);


    }
}
