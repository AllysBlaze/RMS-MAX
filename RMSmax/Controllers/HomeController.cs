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
        private IWebHostEnvironment Environment;
        public int ArticlesPageSize => 6;
        public int EmployeesPageSize => 10;

        public HomeController(IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env)
        {
            articlesRepo = artsRepo;
            employeesRepo = empRepo;
            studentsTimetableRepo = timetableRepo;
            this.subjectRepo = subjectRepo;
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
            Environment = env;
        }
        public IActionResult Index(int page = 1)
        {
            return View(new ArticlesListViewModel
            {
                Faculty = facultyInfo,
                Articles = articlesRepo.Articles.OrderByDescending(a => a.Id).Skip((page - 1) * ArticlesPageSize).Take(ArticlesPageSize),
                PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = ArticlesPageSize, TotalItems = articlesRepo.Articles.Count() }
            });
        }

        public IActionResult Article(int articleId)
        {
            Article art = articlesRepo.Articles.Where(x => x.Id == articleId).FirstOrDefault();
            if (art != null)
                return View(new AricleViewModel
                {
                    Faculty = facultyInfo,
                    Article = art
                });
            else
                return RedirectToAction("Index");
        }

        public IActionResult Studies(string course, int? degree = null, int? semester = null)
        {
            Course c = facultyInfo.Courses.Where(x => x.Name == course).FirstOrDefault();
            if (c != null)
                return View(new StudiesViewModel(Environment) {
                    Faculty = facultyInfo,
                    Course = c,
                    StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == course),
                    Subjects = subjectRepo.Subjects.Where(x => x.Course == course && (degree == null || semester == null ? true : x.Degree == degree && x.Semester == semester))//!!!
                });
            else
                return RedirectToAction("Index");

        }

        public IActionResult EmployeeList(int page = 1, string name = "", string surname="")
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname))
            {
                employees = employeesRepo.Employees.OrderBy(x => x.LastName).Skip((page - 1) * EmployeesPageSize).Take(EmployeesPageSize);
            }
            else
            {
                employees = employeesRepo.Employees.Where(x => x.LastName.Contains(surname) && x.Name.Contains(name)).OrderBy(x => x.LastName).Skip((page - 1) * EmployeesPageSize).Take(EmployeesPageSize);
            }

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = EmployeesPageSize,
                TotalItems = string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) ? employeesRepo.Employees.Count() : employeesRepo.Employees.Where(x => x.LastName.Contains(surname) && x.Name.Contains(name)).Count()
            };

            return View(new EmployeeListViewModel
            {
                Faculty = facultyInfo,
                Employees = employees,
                PagingInfo = pagingInfo,
                CurrentSearchingName = name,
                CurrentSearchingSurname = surname
            }); 
        }

        public IActionResult Contact()
        {
            return View(new ContactViewModel
            {
                Faculty = facultyInfo
            });
        }
    }
}
