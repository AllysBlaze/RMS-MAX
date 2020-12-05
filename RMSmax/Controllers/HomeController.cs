using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using RMSmax.Models;
using RMSmax.Models.ViewModels;

namespace RMSmax.Controllers
{
    public class HomeController : Controller
    {
        private IArticleRepository articlesRepo;
        private IEmployeeRepository employeesRepo;
        private IStudentsTimetableRepository studentsTimetableRepo;
        private ISubjectRepository subjectRepo;
        private Faculty facultyInfo;
        public int PageSize => 5;

        public HomeController(IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env)
        {
            articlesRepo = artsRepo;
            employeesRepo = empRepo;
            studentsTimetableRepo = timetableRepo;
            this.subjectRepo = subjectRepo;
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
        }
        public IActionResult Index(int page = 1)
        {
            return View(new ArticlesListViewModel
            {
                FacultyCourses = facultyInfo.Courses,
                Articles = articlesRepo.Articles.OrderByDescending(a => a.Id).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = PageSize, TotalItems = articlesRepo.Articles.Count() }
            });
        }

        public IActionResult Article(int articleId)
        {
            Article art = articlesRepo.Articles.Where(x => x.Id == articleId).FirstOrDefault();
            if (art != null)
                return View(new AricleViewModel
                {
                    FacultyCourses = facultyInfo.Courses,
                    Article = art
                });
            else
                return RedirectToAction("Index");
        }

        public IActionResult Studies(string course) // TO DO
        {
            Course c = facultyInfo.Courses.Where(x => x.Name == course).FirstOrDefault();
            if(c != null)
                return View(new StudiesViewModel { FacultyCourses = facultyInfo.Courses, Course = c});
            else
                return RedirectToAction("Index");

        }

        public IActionResult EmployeeList(int page = 1, string searching = "") // TO DO
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(searching))
            {
                employees = employeesRepo.Employees.OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);
            }
            else
            {
                employees = null;
            }

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = employeesRepo.Employees.Count()
            };

            return View(new EmployeeListViewModel
            {
                Employees = employees,
                PagingInfo = pagingInfo
            }); 
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel
            {
                FacultyCourses = facultyInfo.Courses,
                FacultyInfo = facultyInfo
            });
        }
    }
}
