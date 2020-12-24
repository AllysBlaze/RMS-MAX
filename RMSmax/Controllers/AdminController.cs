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
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Runtime;
using System.Threading;

namespace RMSmax.Controllers
{
    //[Authorize]
    public class AdminController : Controller
    {
        private IArticleRepository articlesRepo;
        private IEmployeeRepository employeesRepo;
        private IStudentsTimetableRepository studentsTimetableRepo;
        private ISubjectRepository subjectRepo;
        private Faculty facultyInfo;
        private IWebHostEnvironment Environment;
        public int PageSize => 15;
        public AdminController(IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env)
        {
            articlesRepo = artsRepo;
            employeesRepo = empRepo;
            studentsTimetableRepo = timetableRepo;
            this.subjectRepo = subjectRepo;
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
            Environment = env;
        }

        #region Index(FacultyInfo)
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
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            logoFile.CopyTo(fs);
                        }
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
        public IActionResult AddCourse(string NewCourseName)
        {
            if (string.IsNullOrEmpty(NewCourseName) || facultyInfo.Courses.Where(x => x.Name == NewCourseName).FirstOrDefault() != null)
                return View("Index", new IndexViewModel() { Faculty = facultyInfo, NewCourseName = NewCourseName });
            else
            {
                facultyInfo.Courses.Add(new Course() { Name = NewCourseName, FirstDegreeSpecialties = new List<string>(), SecondDegreeSpecialties = new List<string>() });
                facultyInfo.Serialize();

                if(!System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", NewCourseName)))
                    System.IO.Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", NewCourseName));
                if (!System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "studyPlans", NewCourseName)))
                    System.IO.Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "files", "studyPlans", NewCourseName));

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", NewCourseName);

                return RedirectToAction("EditCourse", "Admin", routeValues);
            }
        }

        [HttpPost]
        public IActionResult DeleteCourse(string courseName, string scroll)
        {
            Course course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault();
            if (course != null)
            {
                //usun plany zajec
                int[] timetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == course.Name).Select(x => x.Id).ToArray();
                foreach(var v in timetables)
                {
                    studentsTimetableRepo.DeleteStudentsTimetable(v);
                }
                //usun przedmioty
                int[] subjects = subjectRepo.Subjects.Where(x => x.Course == course.Name).Select(x => x.Id).ToArray();
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
                facultyInfo.Serialize();
            }

            return RedirectToAction("Index", "Admin", scroll);
        }
        #region skladowe edycji
        [HttpPost]
        public IActionResult EditCourseName(string previousName, string newName)
        {
            Course course = facultyInfo.Courses.Where(x => x.Name == previousName).FirstOrDefault();
            if (course != null)
            {
                course.Name = newName;
                facultyInfo.Serialize();
                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", course.Name);

                return RedirectToAction("EditCourse", "Admin", routeValues);
            }
            else
            {
                return View("EditCourse", new EditCourseViewModel(Environment)
                {
                    Faculty = facultyInfo,
                    Course = facultyInfo.Courses.Where(x => x.Name == previousName).FirstOrDefault(),
                    StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == previousName)
                });
            }
        }

        [HttpPost]
        public IActionResult AddSpeciality(string courseName, string spec, int degree, string scroll)
        {
            Course course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault();
            if (course != null)
            {
                if (!string.IsNullOrEmpty(spec))
                {
                    if (degree == 1 && !course.FirstDegreeSpecialties.Contains(spec))
                    {
                        course.FirstDegreeSpecialties.Add(spec);
                        facultyInfo.Serialize();
                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);

                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                    else if (degree == 2 && !course.SecondDegreeSpecialties.Contains(spec))
                    {
                        course.SecondDegreeSpecialties.Add(spec);
                        facultyInfo.Serialize();
                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);

                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                }
            }

            return View("EditCourse", new EditCourseViewModel(Environment)
            {
                Faculty = facultyInfo,
                Course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault(),
                StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == courseName)
            });
        }

        [HttpPost]
        public IActionResult DeleteSpeciality(string courseName, string spec, int degree, string scroll)
        {
            Course course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault();
            if (course != null)
            {
                if (!string.IsNullOrEmpty(spec))
                {
                    if (degree == 1 && course.FirstDegreeSpecialties.Contains(spec))
                    {
                        course.FirstDegreeSpecialties.Remove(spec);
                        facultyInfo.Serialize();
                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);

                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                    else if (degree == 2 && course.SecondDegreeSpecialties.Contains(spec))
                    {
                        course.SecondDegreeSpecialties.Remove(spec);
                        facultyInfo.Serialize();
                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);

                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                }
            }

            return View("EditCourse", new EditCourseViewModel(Environment)
            {
                Faculty = facultyInfo,
                Course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault(),
                StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == courseName)
            });
        }

        [HttpPost]
        public IActionResult AddTimetable(StudentsTimetable studentsTimetable, string scroll)
        {
            if (ModelState.IsValid)
            {
                //if (facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault() != null && studentsTimetable != null)
                if(studentsTimetable != null)
                {
                    
                    studentsTimetableRepo.AddStudentsTimetable(studentsTimetable);
                    
                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", studentsTimetable.Course);

                    return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                }
            }

            return View("EditCourse", new EditCourseViewModel(Environment)
            {
                Faculty = facultyInfo,
                Course = facultyInfo.Courses.Where(x => x.Name == studentsTimetable.Course).FirstOrDefault(),
                StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == studentsTimetable.Course)
            });
        }

        [HttpPost]
        public IActionResult DeleteTimetable(string courseName, int studentsTimetableId, string scroll)
        {
            if (facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault() != null)
            {
                studentsTimetableRepo.DeleteStudentsTimetable(studentsTimetableId);

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", courseName);

                return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
            }

            return View("EditCourse", new EditCourseViewModel(Environment)
            {
                Faculty = facultyInfo,
                Course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault(),
                StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == courseName)
            });
        }

        [HttpPost]
        public IActionResult AddStudyPlan(string courseName, IFormFile file, string scroll)
        {
            if (facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault() != null && Path.GetExtension(file.FileName) == ".pdf")
            {
                string path = Path.Combine(Environment.WebRootPath, "files", "studyPlans", courseName, file.FileName);
                if (!System.IO.File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }

                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", courseName);

                    return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                }
            }

            return View("EditCourse", new EditCourseViewModel(Environment)
            {
                Faculty = facultyInfo,
                Course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault(),
                StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == courseName)
            });
        }

        [HttpPost]
        public IActionResult DeleteStudyPlan(string courseName, string file, string scroll)
        {
            if (facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault() != null)
            {
                string path = Path.Combine(Environment.WebRootPath, "files", "studyPlans", courseName, file);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);

                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", courseName);

                    return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                }
            }

            return View("EditCourse", new EditCourseViewModel(Environment)
            {
                Faculty = facultyInfo,
                Course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault(),
                StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == courseName)
            });
        }
        #endregion
        #endregion

        #region Article
        [HttpGet]
        public IActionResult ArticleList(int page = 1, string title = "", string author = "", DateTime from = new DateTime(), DateTime to = new DateTime())
        {
            IEnumerable<Article> articles = articlesRepo.Articles;
            if (!string.IsNullOrEmpty(title))
            {
                articles = articles.Where(x => x.Title.Contains(title));
            }
            if (!string.IsNullOrEmpty(author))
            {
                articles = articles.Where(x => x.Author.Contains(author));
            }
            if (from != new DateTime())
            {
                articles = articles.Where(x => x.DateTime >= from);
            }
            if (to != new DateTime())
            {
                articles = articles.Where(x => x.DateTime <= to.AddDays(1));
            }
            articles = articles.OrderByDescending(x => x.DateTime).Skip((page - 1) * PageSize).Take(PageSize);

            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = articles.Count()
            };

            return base.View(new ArticleListViewModel() {
                Faculty = facultyInfo,
                Articles = articles,
                PagingInfo = pagingInfo
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
            return View("EditArticle", new EditArticleViewModel() { Faculty = facultyInfo, Article = new Article() { Content = "Podaj treść artykułu" } }) ;
        }

        [HttpPost]
        public IActionResult EditArticle(Article article, IFormFile photoIn = null, IFormFile photoCover = null)
        {
            if (ModelState.IsValid)
            {
                //zabezpieczenie XSS
                if (!XSSValidate(article.Content))
                {
                    return View("EditArticle", new EditArticleViewModel() { Faculty = facultyInfo, Article = article, PhotoIn = photoIn, PhotoCover = photoCover });
                }

                string path = Path.Combine(Environment.WebRootPath, "pictures", "picsArticle");
                if (!System.IO.Directory.Exists(Path.Combine(path, article.Id.ToString())))
                    System.IO.Directory.CreateDirectory(Path.Combine(path, article.Id.ToString()));

                if (photoIn != null)
                {
                    if (!string.IsNullOrEmpty(article.PhotoIn) && System.IO.File.Exists(Path.Combine(path, article.Id.ToString(), article.PhotoIn)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(path, article.Id.ToString(), article.PhotoIn));
                        }
                        catch (Exception) 
                        {

                            return View("EditArticle", new EditArticleViewModel() { Faculty = facultyInfo, Article = article, PhotoIn = photoIn, PhotoCover = photoCover });
                        }
                    using (FileStream fs = new FileStream(Path.Combine(path, article.Id.ToString(), photoIn.FileName), FileMode.Create))
                    {
                        photoIn.CopyTo(fs);
                    }

                    article.PhotoIn = photoIn.FileName;      
                }
                if (photoCover != null)
                {
                    if (!string.IsNullOrEmpty(article.PhotoCover) && System.IO.File.Exists(Path.Combine(path, article.Id.ToString(), article.PhotoCover)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(path, article.Id.ToString(), article.PhotoCover));

                        }
                        catch (Exception)
                        {

                            return View("EditArticle", new EditArticleViewModel() { Faculty = facultyInfo, Article = article, PhotoIn = photoIn, PhotoCover = photoCover });
                        }
                    using (FileStream fs = new FileStream(Path.Combine(path, article.Id.ToString(), photoCover.FileName), FileMode.Create))
                    {
                        photoCover.CopyTo(fs);
                    }

                    article.PhotoCover = photoCover.FileName;
                }

                if (article.Id == 0)
                {
                    articlesRepo.AddArticle(article);
                    int? id = articlesRepo.Articles.Where(x => x.Title == article.Title && x.DateTime == article.DateTime).Select(x => x.Id).FirstOrDefault();
                    if (id != null)
                    {
                        System.IO.Directory.Move(Path.Combine(path, 0.ToString()), Path.Combine(path, id.ToString()));
                    }
                }
                else
                {
                    articlesRepo.EditArticle(article);
                }

                return RedirectToAction("ArticleList");
            }
            else
            {
                return View("EditArticle", new EditArticleViewModel() { Faculty = facultyInfo, Article = article, PhotoIn = photoIn, PhotoCover = photoCover });
            }
        }

        [HttpPost]
        public IActionResult DeleteArticle(int id)
        {
            Article article = articlesRepo.Articles.Where(x => x.Id == id).FirstOrDefault();
            if (article != null)
            {
                if (System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", id.ToString())))
                {
                    try
                    {
                        System.IO.Directory.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", id.ToString()), true);
                    }
                    catch (Exception) { }
                }
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
                if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(surname))
                {
                    employees = employeesRepo.Employees.Where(x => x.LastName.Contains(surname)).OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);
                }
                if (string.IsNullOrEmpty(surname) && !string.IsNullOrEmpty(name)) 
                {
                    employees = employeesRepo.Employees.Where(x => x.Name.Contains(name)).OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);
                }
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
                string path = Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee");
                if (!System.IO.Directory.Exists(Path.Combine(path, employee.Id.ToString())))
                    System.IO.Directory.CreateDirectory(Path.Combine(path, employee.Id.ToString()));

                if (photo != null)
                {
                    if (!string.IsNullOrEmpty(employee.Photo) && System.IO.File.Exists(Path.Combine(path, employee.Id.ToString(), employee.Photo)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(path, employee.Id.ToString(), employee.Photo));
                        }
                        catch (Exception) 
                        {
                            return View("EditEmployee", new EditEmployeeViewModel() { Faculty = facultyInfo, Employee = employee });
                        }
                    using (FileStream fs = new FileStream(Path.Combine(path, employee.Id.ToString(), photo.FileName), FileMode.Create))
                    {
                        photo.CopyTo(fs);
                    }

                    employee.Photo = photo.FileName;
                }

                if (employee.Id == 0)
                {
                    employeesRepo.AddEmployee(employee);
                    int? id = employeesRepo.Employees.Where(x => x.Name == employee.Name && x.LastName == employee.LastName && x.Mail == employee.Mail).Select(x => x.Id).FirstOrDefault();
                    if (id != null)
                    {
                        System.IO.Directory.Move(Path.Combine(path, 0.ToString()), Path.Combine(path, id.ToString()));
                    }
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
            if (employee != null)
            {
                if (System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", id.ToString())))
                {
                    try
                    {
                        System.IO.Directory.Delete(Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", id.ToString()), true);
                    }
                    catch (Exception) { }
                }
            }
            employeesRepo.DeleteEmployee(id);

            return RedirectToAction("EmployeeList");
        }
        #endregion

        #region Subject
        [HttpGet]
        public IActionResult SubjectsList(string course)
        {
            Console.WriteLine();
            return View(new SubjectsListViewModel() { Faculty = facultyInfo, Subjects = subjectRepo.Subjects.Where(x => x.Course == course), CourseName = course});
        }
        [HttpGet]
        public IActionResult EditSubject(int subjectId)
        {
            Subject subject = subjectRepo.Subjects.Where(x => x.Id == subjectId).FirstOrDefault();
            if (subject != null)
                return View(new EditSubjectViewModel() { Faculty = facultyInfo, Subject = subject});
            else
                return RedirectToAction("Index");
        }

        public IActionResult AddSubject(string course)
        {
            return View("EditSubject", new EditSubjectViewModel() { Faculty = facultyInfo, Subject = new Subject() { Course = course } });
        }

        [HttpPost]
        public IActionResult EditSubject(Subject subject, IFormFile doc = null)
        {
            if (ModelState.IsValid && subject != null && doc != null)
            {
                string path = Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", subject.Course);
                if (!System.IO.Directory.Exists(Path.Combine(path, subject.Id.ToString())))
                    System.IO.Directory.CreateDirectory(Path.Combine(path, subject.Id.ToString()));

                subject.File = doc.FileName;
                using (FileStream fs = new FileStream(Path.Combine(path, subject.Id.ToString(), doc.FileName), FileMode.Create))
                {
                    doc.CopyTo(fs);
                }
                if (subject.Id == 0)
                {
                    subjectRepo.AddSubject(subject);
                    int? id = subjectRepo.Subjects.Where(x => x.Name == subject.Name && x.Course == subject.Course && x.Degree == subject.Degree && x.Semester == subject.Semester).Select(x => x.Id).FirstOrDefault();
                    if (id != null)
                    {
                        System.IO.Directory.Move(Path.Combine(path, 0.ToString()), Path.Combine(path, id.ToString()));
                    }
                }
                else
                {
                    subjectRepo.EditSubject(subject);
                }
                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", subject.Course);

                return RedirectToAction("SubjectsList", "Admin", routeValues);
            }
            else if (subject != null)
            {
                return View("EditSubject", new EditSubjectViewModel() { Faculty = facultyInfo, Subject = subject});
            }
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteSubject(int subjectId)
        {
            Subject subject = subjectRepo.Subjects.Where(x => x.Id == subjectId).FirstOrDefault();
            if (subject != null)
            {
                if (System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", subject.Course, subjectId.ToString())))
                {
                    try
                    {
                        System.IO.Directory.Delete(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", subject.Course, subjectId.ToString()), true);
                    }
                    catch (Exception) { }
                }
                subjectRepo.DeleteSubject(subjectId);
                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", subject.Course);

                return RedirectToAction("SubjectsList", "Admin", routeValues);
            }
            else
                return RedirectToAction("Index");
        }
        #endregion

        //!

        [HttpGet]
        public IActionResult Login()
        {
            return View(new MainViewModel() { Faculty = facultyInfo});
        }

        [HttpGet]
        public IActionResult AccountsList()
        {
            return View(new MainViewModel() { Faculty = facultyInfo});
        }


        //zabezpieczenie XSS
        private bool XSSValidate(string input)
        {
            return true;
        }
    }
}
