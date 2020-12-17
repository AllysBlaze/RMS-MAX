using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using RMSmax.Models;
using RMSmax.Models.ViewModels;
using RMSmax.Models.ViewModels.Admin;
using System.IO;

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
        public int PageSize => 20;
        public AdminController(IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env)
        {
            articlesRepo = artsRepo;
            employeesRepo = empRepo;
            studentsTimetableRepo = timetableRepo;
            this.subjectRepo = subjectRepo;
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
            Environment = env;
        }

        #region Index(FacultyInfo, NewCourse)
        [HttpGet]
        public IActionResult Index()
        {
            return View(new IndexViewModel() { Faculty = facultyInfo });
        }

        [HttpPost]
        public IActionResult EditFacultyInfo(Faculty faculty, IFormFile logoFile)
        {
            if (ModelState.IsValid)
            {
                if (logoFile != null)
                {
                    try
                    {
                        System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "pictures", "logo", facultyInfo.Logo));
                        string path = Path.Combine(Environment.WebRootPath, "pictures", "logo", logoFile.FileName);
                        logoFile.CopyTo(new FileStream(path, FileMode.Create));
                    }
                    catch (Exception)
                    {
                        return View("Index", new IndexViewModel() { Faculty = facultyInfo, LogoFile = logoFile });
                    }

                    facultyInfo.Logo = logoFile.FileName;
                }
                facultyInfo.Name = faculty.Name;
                facultyInfo.Street = faculty.Street;
                facultyInfo.Postcode = faculty.Postcode;
                facultyInfo.City = faculty.City;
                facultyInfo.State = faculty.State;
                facultyInfo.Phone = faculty.Phone;
                facultyInfo.Email = faculty.Email;
                facultyInfo.MapSource = faculty.MapSource;
                facultyInfo.Color = faculty.Color;
                facultyInfo.Serialize();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", new IndexViewModel() { Faculty = facultyInfo, LogoFile = logoFile });
            }
        }
        #endregion

        #region Course
        [HttpGet]
        public IActionResult EditCourse(string course)
        {
            if (facultyInfo.Courses.Where(x => x.Name == course).FirstOrDefault() != null)
            {
                return View(new EditCourseViewModel(Environment)
                {
                    Faculty = facultyInfo,
                    Course = facultyInfo.Courses.Where(x => x.Name == course).FirstOrDefault(),
                    StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == course)
                });
            }
            else
                return View("Index", new IndexViewModel() { Faculty = facultyInfo });
        }

        [HttpPost]
        public IActionResult AddCourse(string name)
        {
            if (string.IsNullOrEmpty(name) || facultyInfo.Courses.Where(x => x.Name == name).FirstOrDefault() != null)
                return View("Index", new IndexViewModel() { Faculty = facultyInfo });
            else
            {
                facultyInfo.Courses.Add(new Course() { Name = name, FirstDegreeSpecialties = new List<string>(), SecondDegreeSpecialties = new List<string>() });
                facultyInfo.Serialize();

                if(!System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", name)))
                    System.IO.Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", name));
                if (!System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "studyPlans", name)))
                    System.IO.Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "files", "studyPlans", name));

                return RedirectToAction("EditCourse", "Admin", name);
            }
        }

        [HttpPost]
        public IActionResult DeleteCourse(string courseName)
        {
            Course course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault();
            if (course != null)
            {
                //usun plany zajec
                IEnumerable<StudentsTimetable> timetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == course.Name);
                foreach (var v in timetables)
                {
                    studentsTimetableRepo.DeleteStudentsTimetable(v);
                }
                //usun przedmioty
                IEnumerable<Subject> subjects = subjectRepo.Subjects.Where(x => x.Course == course.Name);
                foreach (var v in subjects)
                {
                    subjectRepo.DeleteSubject(v);
                }
                if (System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", course.Name)))
                {
                    try
                    {
                        System.IO.Directory.Delete(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", course.Name), true);
                    }
                    catch (Exception) { }
                }
                //usun plany studiow
                if (System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "studyPlans", course.Name)))
                {
                    try
                    {
                        System.IO.Directory.Delete(Path.Combine(Environment.WebRootPath, "files", "studyPlans", course.Name), true);
                    }
                    catch (Exception) { }
                }
                //usun kierunek
                facultyInfo.Courses.Remove(course);
            }

            return RedirectToAction("Index");
        }
        #region skladowe edycji
        [HttpPost]
        public IActionResult EditCourse(Course course, IList<StudentsTimetable> timetables)
        {
            //zapis danych
            return RedirectToAction("Index");
        }
        #endregion
        #endregion

        #region Article
        [HttpGet]
        public IActionResult ArticleList(int page = 1)// TO DO Wyszukiwanie
        {
            return base.View(new ArticleListViewModel() {
                Faculty = facultyInfo,
                Articles = articlesRepo.Articles.OrderByDescending(a => a.DateTime).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo { CurrentPage = page, ItemsPerPage = PageSize, TotalItems = articlesRepo.Articles.Count() }
            });
        }

        [HttpGet]
        public IActionResult EditArticle(int id)
        {
            return View(new EditArticleViewModel() { Faculty = facultyInfo, Article  = articlesRepo.Articles.FirstOrDefault(x => x.Id == id)});
        }

        [HttpGet]
        public IActionResult AddArticle()
        {
            return View("EditArticle", new EditArticleViewModel() { Faculty = facultyInfo, Article = new Article() }) ;
        }

        [HttpPost]
        public IActionResult EditArticle(Article article, IFormFile photoIn = null, IFormFile photoCover = null)
        {
            if (ModelState.IsValid)
            {
                if (photoIn != null)
                {
                    if (!string.IsNullOrEmpty(article.PhotoIn) && System.IO.File.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoIn)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoIn));
                        }
                        catch (Exception) { }

                    article.PhotoIn = photoIn.FileName;
                    photoIn.CopyTo(new FileStream(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", photoIn.FileName), FileMode.Create));
                }
                if (photoCover != null)
                {
                    if (!string.IsNullOrEmpty(article.PhotoCover) && System.IO.File.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoCover)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoCover));
                        }
                        catch (Exception) { }

                    article.PhotoCover = photoCover.FileName;
                    photoCover.CopyTo(new FileStream(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", photoCover.FileName), FileMode.Create));
                }

                if (article.Id == 0)
                {
                    articlesRepo.AddArticle(article);
                }
                else
                {
                    articlesRepo.EditArticle(article);
                }

                return RedirectToAction("ArticleList");
            }
            else
            {
                return View("EditArticle", new EditArticleViewModel() { Faculty = facultyInfo, Article = article }); ;
            }
        }

        [HttpPost]
        public IActionResult DeleteArticle(int id)
        {
            Article article = articlesRepo.Articles.Where(x => x.Id == id).FirstOrDefault();
            if (article != null)
            {
                if (!string.IsNullOrEmpty(article.PhotoIn) && System.IO.File.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoIn)))
                    try
                    {
                        System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoIn));
                    }
                    catch (Exception) { }

                if (!string.IsNullOrEmpty(article.PhotoCover) && System.IO.File.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoCover)))
                    try
                    {
                        System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", article.PhotoCover));
                    }
                    catch (Exception) { }
            }
            articlesRepo.DeleteArticle(id);
            return RedirectToAction("ArticleList");
        }
        #endregion

        #region Employee
        [HttpGet]
        public IActionResult EmployeeList(int page = 1, string name = "", string surname = "")
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname))
            {
                employees = employeesRepo.Employees.OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);
            }
            else
            {
                employees = employeesRepo.Employees.Where(x => x.LastName.Contains(surname) && x.Name.Contains(name)).OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);
            }
            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) ? employeesRepo.Employees.Count() : employeesRepo.Employees.Where(x => x.LastName.Contains(surname) && x.Name.Contains(name)).Count()
            };

            return View(new EmployeesListViewModel()
            {
                Faculty = facultyInfo,
                Employees = employees,
                PagingInfo = pagingInfo
            });
        }

        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            return View(new EditEmployeeViewModel() { Faculty = facultyInfo, Employee = employeesRepo.Employees.FirstOrDefault(x => x.Id == id) });
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View("EditEmployee", new EditEmployeeViewModel() { Faculty = facultyInfo, Employee = new Employee() }) ;
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee employee, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    if (!string.IsNullOrEmpty(employee.Photo) && System.IO.File.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", employee.Photo)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", employee.Photo));
                        }
                        catch (Exception) { }

                    employee.Photo = photo.FileName;
                    photo.CopyTo(new FileStream(Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", photo.FileName), FileMode.Create));
                }

                if (employee.Id == 0)
                {
                    employeesRepo.AddEmployee(employee);
                }
                else
                {
                    employeesRepo.EditEmployee(employee);
                }

                return RedirectToAction("EmployeeList");
            }
            else
            {
                return View("EditEmployee", new EditEmployeeViewModel() { Faculty = facultyInfo, Employee = employee }); ;
            }
        }

        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {
            Employee employee = employeesRepo.Employees.Where(x => x.Id == id).FirstOrDefault();
            if(employee != null)
                if (!string.IsNullOrEmpty(employee.Photo) && System.IO.File.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", employee.Photo)))
                    try
                    {
                        System.IO.File.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", employee.Photo));
                    }
                    catch (Exception) { }

            employeesRepo.DeleteEmployee(id);
            return RedirectToAction("EmployeeList");
        }
        #endregion

        //!

        [HttpGet]
        public IActionResult SubjectsList(string course)
        {
            return View(new SubjectsListViewModel() { Faculty = facultyInfo, Subjects = subjectRepo.Subjects.Where(x => x.Course == course)});
        }
        [HttpGet]
        public IActionResult EditSubject()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new MainViewModel() { Faculty = facultyInfo, });
        }
    }
}
