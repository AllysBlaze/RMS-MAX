using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using RMSmax.Models;
using RMSmax.Models.EventLog;
using RMSmax.Models.ViewModels;
using RMSmax.Models.ViewModels.Admin;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Identity.Core;
using RMSmax.Models.EventLog;
using RMSmax.Data;

namespace RMSmax.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IArticleRepository articlesRepo;
        private IEmployeeRepository employeesRepo;
        private IStudentsTimetableRepository studentsTimetableRepo;
        private ISubjectRepository subjectRepo;
        private Faculty facultyInfo;
        private IWebHostEnvironment Environment;
        private ILogger logger;
        private UserManager<IdentityUser> userManager;
        private AppIdentityDbContext context;
        public int PageSize => 15;
        public AdminController(UserManager<IdentityUser> user, IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env, ILoggerFactory loggerFactory, AppIdentityDbContext _context)
        {
            EventLogs.Initialize(env, loggerFactory);
            articlesRepo = artsRepo;
            employeesRepo = empRepo;
            studentsTimetableRepo = timetableRepo;
            this.subjectRepo = subjectRepo;
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
            Environment = env;
            logger = loggerFactory.CreateLogger("AdminController");
            userManager = user;
            context = _context;
        }

        #region Index(FacultyInfo)
        [HttpGet]
        public IActionResult Index()
        {
            return View(new IndexViewModel(Environment) { Faculty = facultyInfo}) ;
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
                        EventLogs.LogError("Nie udało się zmienić informacji o wydziale.", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
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

                EventLogs.LogInformation("Zmieniono informacje o wydziale.");
                EventLogs.LogWarning("warn.");
                EventLogs.LogError("Blad. a");

                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", new IndexViewModel(Environment) { Faculty = facultyInfo, LogoFile = logoFile });
            }
        }

        [HttpPost]
        public IActionResult UploadSliderPhoto(int id, IFormFile photo)
        {
            if (id == 1 || id == 2 || id == 3)
            {
                if (photo != null)
                {
                    try
                    {
                        string dir = Path.Combine(Environment.WebRootPath, "pictures", "picsSlider", id.ToString());
                        string[] files = Directory.GetFiles(Path.Combine(Environment.WebRootPath, "pictures", "picsSlider", id.ToString()));
                        foreach (var v in files)
                        {
                            System.IO.File.Delete(v);
                        }
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }
                        string path = Path.Combine(dir, photo.FileName);
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            photo.CopyTo(fs);
                        }

                    }
                   catch (Exception)
                    {
                        EventLogs.LogError("Nie udało się dodać zdjęcia do banera strony głównej.", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
                    }
                }

                EventLogs.LogInformation("Zmieniono " + id + " zdjęcie banera strony głównej.");

                return RedirectToAction("Index");
            }
            else
            {
                EventLogs.LogError("Nie udało się dodać zdjęcia do banera strony głównej.", "Błąd serwera.");
                return RedirectToAction("EventLog");
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
            {
                EventLogs.LogError("Nie udało się dodać kierunku studiów.", "Kierunek o tej nazwie już istnieje.");
                return RedirectToAction("EventLog");
            }
            else
            {
                facultyInfo.Courses.Add(new Course() { Name = NewCourseName, FirstDegreeSpecialties = new List<string>(), SecondDegreeSpecialties = new List<string>() });
                facultyInfo.Serialize();

                if (!System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", NewCourseName)))
                    System.IO.Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", NewCourseName));
                if (!System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "studyPlans", NewCourseName)))
                    System.IO.Directory.CreateDirectory(Path.Combine(Environment.WebRootPath, "files", "studyPlans", NewCourseName));

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", NewCourseName);

                EventLogs.LogInformation("Dodano nowy kierunek studiów.", NewCourseName);

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
                EventLogs.LogInformation("Usunięto plany zajęć kieruneku: " + courseName + ".");
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
                    catch (Exception) 
                    {
                        EventLogs.LogError("Nie udało się usunąć kart przedmiotów kieruneku: " + courseName + ".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
                EventLogs.LogInformation("Usunięto karty przedmiotów kieruneku: " + courseName + ".");
                //usun plany studiow
                if (System.IO.Directory.Exists(Path.Combine(Environment.WebRootPath, "files", "studyPlans", course.Name)))
                {
                    try
                    {
                        System.IO.Directory.Delete(Path.Combine(Environment.WebRootPath, "files", "studyPlans", course.Name), true);
                    }
                    catch (Exception) 
                    {
                        EventLogs.LogError("Nie udało się usunąć planów studiów kieruneku: " + courseName + ".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
                EventLogs.LogInformation("Usunięto plany studiów kieruneku: " + courseName + ".");
                //usun kierunek
                facultyInfo.Courses.Remove(course);
                facultyInfo.Serialize();

                EventLogs.LogInformation("Usunięto kierunek studiów:" + courseName + ".");
            }

            return RedirectToAction("Index", "Admin", scroll);
        }
        #region skladowe edycji
        [HttpPost]
        public IActionResult EditCourseName(string previousName, string newName)
        {
            Course course = facultyInfo.Courses.Where(x => x.Name == previousName).FirstOrDefault();
            Course c = facultyInfo.Courses.Where(x => x.Name == newName).FirstOrDefault();
            if (c != null)
            {
                EventLogs.LogError("Nie udało się zmienić nazwy kierunku " + previousName + " na " + newName + ".", "Kierunek o tej nazwie już istnieje.");
                return RedirectToAction("EventLog");
            }
            if (course != null)
            {
                course.Name = newName;
                facultyInfo.Serialize();
                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", course.Name);

                EventLogs.LogInformation("Zmieniono nazwę kierunku z " + previousName + " na " + newName + ".");

                return RedirectToAction("EditCourse", "Admin", routeValues);
            }
            else
            {
                EventLogs.LogError("Nie udało się zmienić nazwy kierunku " + previousName + " na " + newName + ".", "Błąd serwera.");
                return RedirectToAction("EventLog");
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

                        EventLogs.LogInformation("Dodano specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                    else if (degree == 2 && !course.SecondDegreeSpecialties.Contains(spec))
                    {
                        course.SecondDegreeSpecialties.Add(spec);
                        facultyInfo.Serialize();
                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);

                        EventLogs.LogInformation("Dodano specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

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

                        EventLogs.LogInformation("Usunięto specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                    else if (degree == 2 && course.SecondDegreeSpecialties.Contains(spec))
                    {
                        course.SecondDegreeSpecialties.Remove(spec);
                        facultyInfo.Serialize();
                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);

                        EventLogs.LogInformation("Usunięto specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

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
                if(studentsTimetable != null)
                {       
                    studentsTimetableRepo.AddStudentsTimetable(studentsTimetable);
                    
                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", studentsTimetable.Course);

                    EventLogs.LogInformation("Dodano plan zajęć. Stopień: " + studentsTimetable.Degree + ", Semestr: " + studentsTimetable.Semester + ".", "Kierunek: " + studentsTimetable.Course);

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

                EventLogs.LogInformation("Usunięto plan zajęć.", "Kierunek: " + courseName);


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
                    try
                    {
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            file.CopyTo(fs);
                        }
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError("Nie udało się dodać planu studiów (" + file.Name + ") na kieruneku: " + courseName + ".", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
                    }

                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", courseName);

                    EventLogs.LogInformation("Dodano plan studiów ("+ file.Name +").", "Kierunek: " + courseName);

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
                    try
                    {
                        System.IO.File.Delete(path);
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError("Nie udało się usunąć planu studiów (" + file + ") na kieruneku: " + courseName + ".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }

                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", courseName);

                    EventLogs.LogInformation("Usunięto plan studiów (" + file + ").", "Kierunek: " + courseName);

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
        public IActionResult ArticleList(int page = 1, string title = "", string author = "")
        {
            IEnumerable<Article> articles = articlesRepo.Articles;
            if (!string.IsNullOrEmpty(title))
            {
                articles = articles.Where(x => x.Title.ToLower().Contains(title.ToLower()));
            }
            if (!string.IsNullOrEmpty(author))
            {
                articles = articles.Where(x => x.Author.ToLower().Contains(author.ToLower()));
            }
        
            var pagingInfo = new PagingInfo()
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = articles.Count()
            };

            articles = articles.OrderByDescending(x => x.DateTime).Skip((page - 1) * PageSize).Take(PageSize);

            return View(new ArticleListViewModel() {
                Faculty = facultyInfo,
                Articles = articles,
                PagingInfo = pagingInfo,
                CurrentSearchingTitle = title,
                CurrentSearchingAuthor = author
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
                    EventLogs.LogError("Nie udało się dodać aktualności.", "Ochrona przed atakiem XSS.");
                    return RedirectToAction("EventLog");
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
                            EventLogs.LogError("Nie udało się dodać aktualności.", "Problem z plikiem.");
                            return RedirectToAction("EventLog");
                        }
                    try
                    {
                        using (FileStream fs = new FileStream(Path.Combine(path, article.Id.ToString(), photoIn.FileName), FileMode.Create))
                        {
                            photoIn.CopyTo(fs);
                        }
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError("Nie udało się dodać aktualności.", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
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
                            EventLogs.LogError("Nie udało się dodać aktualności.", "Problem z plikiem.");
                            return RedirectToAction("EventLog");
                        }
                    try
                    {
                        using (FileStream fs = new FileStream(Path.Combine(path, article.Id.ToString(), photoCover.FileName), FileMode.Create))
                        {
                            photoCover.CopyTo(fs);
                        }
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError("Nie udało się dodać aktualności.", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
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
                    EventLogs.LogInformation("Dodano nową aktualność.", article.Title);
                }
                else
                {
                    articlesRepo.EditArticle(article);
                    EventLogs.LogInformation("Edycja aktualności", article.Title);
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
                    catch (Exception) 
                    {
                        EventLogs.LogError("Nie udało się usunąć aktualności "+article.Title+".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
            }
            articlesRepo.DeleteArticle(id);

            EventLogs.LogInformation("Usunięto aktualność.", article.Title);

            return RedirectToAction("ArticleList");
        }
        #endregion

        #region Employee
        [HttpGet]
        public IActionResult EmployeeList(int page = 1, string name = "", string surname = "")
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
                ItemsPerPage = PageSize,
                TotalItems = employees.Count()
            };

            employees = employees.OrderBy(x => x.LastName).Skip((page - 1) * PageSize).Take(PageSize);

            return View(new EmployeesListViewModel()
            {
                Faculty = facultyInfo,
                Employees = employees,
                PagingInfo = pagingInfo,
                CurrentSearchingName = name,
                CurrentSearchingSurname = surname
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
                            EventLogs.LogError("Nie udało się dodać/edytować pracownika ("+employee.Name +" " + employee.LastName+").", "Problem z plikiem.");
                            return RedirectToAction("EventLog");
                        }
                    try
                    {
                        using (FileStream fs = new FileStream(Path.Combine(path, employee.Id.ToString(), photo.FileName), FileMode.Create))
                        {
                            photo.CopyTo(fs);
                        }
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError("Nie udało się dodać/edytować pracownika (" + employee.Name + " " + employee.LastName + ").", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
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
                    if(string.IsNullOrEmpty(employee.Photo))
                        EventLogs.LogWarning("Dodano nowego pracownika (" + employee.Name + " " + employee.LastName + ").", "Brak zdjęcia.");
                    else
                        EventLogs.LogInformation("Dodano nowego pracownika (" + employee.Name + " " + employee.LastName + ").");
                }

                return RedirectToAction("EmployeeList");
            }
            else
            {
                EventLogs.LogInformation("Edycja pracownika (" + employee.Name + " " + employee.LastName + ").");
                return View("EditEmployee", new EditEmployeeViewModel() { Faculty = facultyInfo, Employee = employee });
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
                    catch (Exception) 
                    {
                        EventLogs.LogError("Nie udało się usunąć pracownika (" + employee.Name + " " + employee.LastName + ").", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
            }
            employeesRepo.DeleteEmployee(id);

            EventLogs.LogInformation("Usunięto pracownika pracownika (" + employee.Name + " " + employee.LastName + ").");

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
                try
                {
                    using (FileStream fs = new FileStream(Path.Combine(path, subject.Id.ToString(), doc.FileName), FileMode.Create))
                    {
                        doc.CopyTo(fs);
                    }
                }
                catch (Exception)
                {
                    EventLogs.LogError("Nie udało się edytować/dodać przedmiotu (" + subject.Name + ").", "Problem z plikiem.");
                    return RedirectToAction("EventLog");
                }
                if (subject.Id == 0)
                {
                    subjectRepo.AddSubject(subject);
                    int? id = subjectRepo.Subjects.Where(x => x.Name == subject.Name && x.Course == subject.Course && x.Degree == subject.Degree && x.Semester == subject.Semester).Select(x => x.Id).FirstOrDefault();
                    if (id != null)
                    {
                        System.IO.Directory.Move(Path.Combine(path, 0.ToString()), Path.Combine(path, id.ToString()));
                    }
                    EventLogs.LogInformation("Dodano nowy przedmiot (" + subject.Name + ").", "Kierunek: " + subject.Course);
                }
                else
                {
                    subjectRepo.EditSubject(subject);
                    EventLogs.LogInformation("Edytowano przedmiot (" + subject.Name + ").", "Kierunek: " + subject.Course);
                }
                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", subject.Course);

                return RedirectToAction("SubjectsList", "Admin", routeValues);
            }
            else if (subject != null)
            {
                return View("EditSubject", new EditSubjectViewModel() { Faculty = facultyInfo, Subject = subject });
            }
            else
            {
                EventLogs.LogError("Nie udało się edytować/dodać przedmiotu (" + subject.Name + ").", "Przedmiot nie istnieje");
                return RedirectToAction("EventLog");
            }
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
                    catch (Exception) 
                    {
                        EventLogs.LogError("Nie udało się usunąć przedmiotu (" + subject.Name + ").", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
                subjectRepo.DeleteSubject(subjectId);
                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", subject.Course);

                EventLogs.LogInformation("usunięto przedmiot (" + subject.Name + ").", "Kierunek: " + subject.Course);

                return RedirectToAction("SubjectsList", "Admin", routeValues);
            }
            else
            {
                EventLogs.LogError("Nie udało się usunąć przedmiotu (" + subject.Name + ").", "Przedmiot nie istnieje");
                return RedirectToAction("EventLog");
            }
        }
        #endregion

        #region Identity
        [HttpPost]
        public async Task <IActionResult> CreateUser(User user, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != confirmPassword)
                {
                    return View("Index", new IndexViewModel() { Faculty = facultyInfo });
                }
                else
                {
                    IdentityUser appUser = new IdentityUser { UserName = user.Name };
                    IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("AccountsList");
                    }
                    else
                    {
                        foreach (IdentityError e in result.Errors)
                        {
                            ModelState.AddModelError("", e.Description); //todo dziennik zdarzen
                        }

                    }
                }
            }
            return RedirectToAction("AccountsList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(IdentityUser user) //Przekazujemy Id czy obiekt user?
        {
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("AccountsList");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika");
            }
            return RedirectToAction("AccountsList");
        }

        [HttpPost]
        public async Task<IActionResult> EditUserPassword(string id,string oldPassword, string newPassword, string newPassword2) //prawdopodobnie będę musiała dopisać walidator hasła
        {
            if(oldPassword == null|| newPassword != newPassword2)
            {
                return View("Index", new IndexViewModel(Environment) { Faculty = facultyInfo });
            }
            else
            {
                
                IdentityUser user = await GetCurrentUserAsync(); 
                if (user != null)
                {
                    if (newPassword != null)
                    {
                        IdentityResult result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            AddErrorsFromResult(result);
                            return View("Index", new IndexViewModel(Environment) { Faculty = facultyInfo });
                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie znaleziono użytkownika");
                    return View("Index", new IndexViewModel(Environment) { Faculty = facultyInfo });
                }
                
            }
            return View("Index", new IndexViewModel(Environment) { Faculty = facultyInfo });
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private Task<IdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        #endregion

        public IActionResult EventLog()
        {
            return View(new EventLogViewModel() { Faculty = facultyInfo, Logs = Models.EventLog.EventLogs.Logs });
        }

        //!

        [HttpGet]
        public IActionResult Login()
        {
            return View(new MainViewModel() { Faculty = facultyInfo});
        }

        [HttpGet]
        public IActionResult AccountsList()
        {
            var users = context.Users.AsQueryable();
            return View(new AccountListViewModel() {
                Faculty = facultyInfo,
                UserList=users
        });
        }


        //zabezpieczenie XSS
        private bool XSSValidate(string input)
        {
            input = System.Web.HttpUtility.HtmlDecode(input);
            string[] legalMarkups = new string[] {"p", "h2", "h3", "h4", "strong", "i", "u", "span", "ul", "li", "ol", "a", "figure", "table", "tbody", "tr", "td"};

            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == '<')
                {
                    if (input[i + 1] == '/')
                        continue;

                    bool result = false; 
                    foreach (var v in legalMarkups)
                    {
                        if (input.Substring(i + 1, v.Length) == v)
                        {
                            result = true;
                            break;
                        }
                    }
                    if (!result)
                        return result;
                }
            }

            return true;
        }


    }
}
