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
        public int PageSize => 6;

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

        public IActionResult Studies(string course, int? degree = null, int? semester = null)
        {
            Course c = facultyInfo.Courses.Where(x => x.Name == course).FirstOrDefault();
            if (c != null)
                return View(new StudiesViewModel(Environment) {
                    FacultyCourses = facultyInfo.Courses,
                    Course = c,
                    StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == course),
                    Subjects = subjectRepo.Subjects.Where(x => x.Course == course && (degree == null || semester == null ? true : x.Degree == degree && x.Semester == semester))//!!!
                });
            else
                return RedirectToAction("Index");

        }

        public IActionResult EmployeeList(int page = 1, string searching = "")
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(searching))
            {
                employees = employeesRepo.Employees.OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);
            }
            else
            {
                employees = employeesRepo.Employees.Where(x => x.LastName.Contains(searching) || x.Name.Contains(searching) || (x.Name + " " + x.LastName).Contains(searching)).OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);
            }

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = string.IsNullOrEmpty(searching) ? employeesRepo.Employees.Count() : employeesRepo.Employees.Where(x => x.LastName.Contains(searching) || x.Name.Contains(searching) || (x.Name + " " + x.LastName).Contains(searching)).Count()
            };

            return View(new EmployeeListViewModel
            {
                Employees = employees,
                PagingInfo = pagingInfo,
                CurrentSearching = searching
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
