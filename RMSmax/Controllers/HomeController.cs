using System.Collections.Generic;
using System.Linq;
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
        public int EmployeesPageSize => 5;

        public HomeController(IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env)
        {
            articlesRepo = artsRepo;
            employeesRepo = empRepo;
            studentsTimetableRepo = timetableRepo;
            this.subjectRepo = subjectRepo;
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
            Environment = env;
        }

        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            return View(new ArticlesListViewModel(Environment)
            {
                Faculty = facultyInfo,
                Articles = articlesRepo.Articles.OrderByDescending(a => a.DateTime).Skip((page - 1) * ArticlesPageSize).Take(ArticlesPageSize),
                PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = ArticlesPageSize, TotalItems = articlesRepo.Articles.Count() }
            });
        }

        [HttpGet]
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
                return NotFound();
        }

        [HttpGet]
        public IActionResult Studies(string course, int? degree = null, int? semester = null)
        {
            Course c = facultyInfo.Courses.Where(x => x.Name == course).FirstOrDefault();
            if (c != null)
            {
                IEnumerable<Subject> subjects = subjectRepo.Subjects; ;
                if (degree is null && semester is null)
                    subjects = new List<Subject>();

                if (degree != null)
                {
                    subjects = subjects.Where(x => x.Degree == degree);
                }
                if (semester != null)
                {
                    subjects = subjects.Where(x => x.Semester == semester);
                }

                return View(new StudiesViewModel(Environment)
                {
                    Faculty = facultyInfo,
                    Course = c,
                    StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == course).OrderBy(x => x.Degree).ThenBy(x => x.Semester),
                    Subjects = subjects.OrderBy(x => x.Name)
                });
            }
            else
                return NotFound();

        }

        [HttpGet]
        public IActionResult EmployeeList(int page = 1, string name = "", string surname="")
        {
            IEnumerable<Employee> employees = employeesRepo.Employees.OrderBy(x => x.LastName);
            if (!string.IsNullOrEmpty(name))
            {
                employees = employees.Where(x => x.Name.ToUpper().Contains(name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(surname))
            {
                employees = employees.Where(x => x.LastName.ToUpper().Contains(surname.ToUpper()));
            }

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = EmployeesPageSize,
                TotalItems = employees.Count()
            };

            employees = employees.OrderBy(x => x.LastName).Skip((page - 1) * EmployeesPageSize).Take(EmployeesPageSize);

            return View(new EmployeeListViewModel
            {
                Faculty = facultyInfo,
                Employees = employees,
                PagingInfo = pagingInfo,
                CurrentSearchingName = name,
                CurrentSearchingSurname = surname
            }); 
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(new MainViewModel
            {
                Faculty = facultyInfo
            });
        }
    }
}
