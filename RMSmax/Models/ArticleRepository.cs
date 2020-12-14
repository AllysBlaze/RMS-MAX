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
        public void AddArticle(Article article)
        {
            context.AddRange(article);
            context.SaveChanges();
        }
        public void DeleteArticle(Article article)
        {
            context.Remove(context.Articles.Single(a => a.Id == article.Id));
            context.SaveChanges();
        }
        public void EditArticle(Article art)
        {
            var article = context.Articles.First(a => a.Id == art.Id);
            article.Author = art.Author;
            article.Content = art.Content;
            article.PhotoCover = art.PhotoCover;
            article.PhotoIn = art.PhotoIn;
            article.Title = art.Title;
            context.SaveChanges();
        }

    }
}
