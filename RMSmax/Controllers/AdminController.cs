using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using RMSmax.Models;
using RMSmax.Models.ViewModels;
using RMSmax.Models.ViewModels.Admin;

namespace RMSmax.Controllers
{
    public class AdminController : Controller
    {
        private IArticleRepository articlesRepo;
        private IEmployeeRepository employeesRepo;
        private IStudentsTimetableRepository studentsTimetableRepo;
        private ISubjectRepository subjectRepo;
        private Faculty facultyInfo;
        private IWebHostEnvironment Environment;
        public AdminController(IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env)
        {
            articlesRepo = artsRepo;
            employeesRepo = empRepo;
            studentsTimetableRepo = timetableRepo;
            this.subjectRepo = subjectRepo;
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
            Environment = env;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new IndexViewModel() { Faculty = facultyInfo });
        }

        [HttpPost]
        public IActionResult EditFacultyInfo(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                facultyInfo.Name = faculty.Name;
                facultyInfo.Street = faculty.Street;
                facultyInfo.Postcode = faculty.Postcode;
                facultyInfo.City = faculty.City;
                facultyInfo.State = faculty.State;
                facultyInfo.Phone = faculty.Phone;
                facultyInfo.Email = faculty.Email;
                facultyInfo.MapSource = faculty.MapSource;
                facultyInfo.Color = faculty.Color;
                facultyInfo.Logo = faculty.Logo;
                facultyInfo.Serialize();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", new IndexViewModel() { Faculty = facultyInfo });
            }
        }

        [HttpGet]
        public IActionResult EditCourse()
        {
            return View(new MainViewModel() { Faculty = facultyInfo });
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            return View("EditCourse", new MainViewModel() { Faculty = facultyInfo });
        }

        [HttpPost]
        public IActionResult EditCourse(Course course, IList<StudentsTimetable> timetables)
        {
            //zapis danych
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteCourse(Course course)
        {
            //usun dane
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ArticleList()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }

        [HttpGet]
        public IActionResult EditArticle()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }

        [HttpGet]
        public IActionResult AddArticle()
        {
            return View("EditArticle");
        }

        [HttpPost]
        public IActionResult EditArticle(Article article)
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }

        [HttpPost]
        public IActionResult DeleteArticle(Article article)
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeList()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }

        [HttpGet]
        public IActionResult EditEmployee()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View("EditEmployee");
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee employee)
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteEmployee()
        {
            return View();
        }











        [HttpGet]
        public IActionResult SubjectsList()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }
        [HttpGet]
        public IActionResult EditSubject()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }
    }
}
