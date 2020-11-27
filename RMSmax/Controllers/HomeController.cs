using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RMSmax.Models;

namespace RMSmax.Controllers
{
    public class HomeController : Controller
    {
        private IArticleRepository articlesRepo;
        public HomeController(IArticleRepository artsRepo)
        {
            articlesRepo = artsRepo;
        }
        public IActionResult Index()
        {
            return View(articlesRepo.Articles);
        }
    }
}
