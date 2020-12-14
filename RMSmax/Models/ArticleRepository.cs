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
        public void AddArticle(string title, string content, string author, string photoCover, string photoIn)
        {
            var article = new Article
            {
                Title = title,
                Content = content,
                Author = author,
                PhotoCover = photoCover,
                PhotoIn = photoIn
            };
            context.AddRange(article);
            context.SaveChanges();
        }
        public void DeleteArticle( int id)
        {
            context.Remove(context.Articles.Single(a => a.Id == id));
            context.SaveChanges();
        }
        public void EditArticle(int id,string title, string content, string author, string photoCover, string photoIn)
        {
            var article = context.Articles.First(a => a.Id == id);
            article.Author = author;
            article.Content = content;
            article.PhotoCover = photoCover;
            article.PhotoIn = photoIn;
            article.Title = title;
            context.SaveChanges();
        }

    }
}
