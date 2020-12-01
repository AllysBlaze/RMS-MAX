using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMSmax.Models;
using RMSmax.Models.ViewModels;

namespace RMSmax.Controllers
{
    public class HomeController : Controller
    {
        private IArticleRepository articlesRepo;
        public int PageSize => 5;
        public HomeController(IArticleRepository artsRepo)
        {
            articlesRepo = artsRepo;
        }
        public IActionResult Index(int page = 1)
        {
            return View(new ArticlesListViewModel
            {
                Articles = articlesRepo.Articles.OrderByDescending(a => a.Id).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = PageSize, TotalItems = articlesRepo.Articles.Count() }
            });
        }

        public IActionResult Article(int articleId)
        {
            Article article = articlesRepo.Articles.Where(x => x.Id == articleId).FirstOrDefault();
            return View(article);
        }
    }
}
