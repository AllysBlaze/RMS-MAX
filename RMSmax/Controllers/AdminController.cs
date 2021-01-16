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
using System.Text.RegularExpressions;
using RMSmax.Data;
using System.Diagnostics;

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
        private RMSContext db;

        public int PageSize => 15;

        public AdminController(UserManager<IdentityUser> user, IArticleRepository artsRepo, IEmployeeRepository empRepo, IStudentsTimetableRepository timetableRepo, ISubjectRepository subjectRepo, IWebHostEnvironment env, ILoggerFactory loggerFactory, AppIdentityDbContext _context, RMSContext _db)
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
            db = _db;
        }

        #region Index(FacultyInfo)
        [HttpGet]
        public IActionResult Index()
        {
            return View(new IndexViewModel(Environment) { Faculty = facultyInfo, CurrentUserName = GetCurrentUserAsync().Result.UserName });
        }

        [HttpPost]
        public IActionResult EditFacultyInfo(Faculty faculty, IFormFile logoFile)
        {
            if (ModelState.IsValid)
            {
                if (!faculty.MapSource.StartsWith("https://www.google.com/maps/embed"))
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić informacji o wydziale.", "Nieprawidłowe źródło mapy.");
                    return RedirectToAction("EventLog");
                }

                if (logoFile != null)
                {
                    string[] legalExts = new string[] {".jpg", ".jpeg", ".png", ".gif", ".svg", ".tiff", ".pjp", ".jfif", ".bmp", ".svgz", ".ico", ".dib", ".tif", ".pjpeg", ".avif"};
                    string extension = Path.GetExtension(logoFile.FileName).ToLower();
                    bool result = false;
                    foreach (var v in legalExts)
                    {
                        if (extension == v)
                        {
                            result = true;
                            break;
                        }
                    }
                    if (!result)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić informacji o wydziale.", "Nieprawidłowy plik z logo wydziału.");
                        return RedirectToAction("EventLog");
                    }

                    string path = Path.Combine(Environment.WebRootPath, "pictures", "logo");
                    try
                    {
                        System.IO.File.Delete(Path.Combine(path, facultyInfo.Logo));
                        path = Path.Combine(path, logoFile.FileName);
                        using (FileStream fs = new FileStream(path, FileMode.Create))
                        {
                            logoFile.CopyTo(fs);
                        }
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić informacji o wydziale.", "Problem z plikiem logo wydziału.");
                        return RedirectToAction("EventLog");
                    }

                    facultyInfo.Logo = logoFile.FileName;
                }
                facultyInfo.Name = faculty.Name.Trim();
                facultyInfo.Street = faculty.Street.Trim();
                facultyInfo.Postcode = faculty.Postcode.Trim();
                facultyInfo.City = faculty.City.Trim();
                facultyInfo.State = faculty.State.Trim();
                facultyInfo.Phone = faculty.Phone.Trim();
                facultyInfo.Email = faculty.Email.Trim();
                facultyInfo.MapSource = faculty.MapSource.Trim();
                facultyInfo.Color = faculty.Color;
                facultyInfo.Serialize();

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Zmieniono informacje o wydziale.");

                return RedirectToAction("Index");
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić informacji o wydziale.");
                return RedirectToAction("EventLog");
            }
        }

        [HttpPost]
        public IActionResult UploadSliderPhoto(int id, IFormFile photo, string scroll)
        {
            if (id >= 1 && id <= 3 && photo != null)
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
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia do banera strony głównej.", "Problem z plikiem.");
                    return RedirectToAction("EventLog");
                }

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Zmieniono " + id + " zdjęcie banera strony głównej.");
                return RedirectToAction("Index", "Admin", scroll);
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia do banera strony głównej.", "Nieprawidłowe dane.");
                return RedirectToAction("EventLog");
            }
        }
        #endregion

        #region Course
        [HttpGet]
        public IActionResult EditCourse(string course)
        {
            Course crs = facultyInfo.Courses.Where(x => x.Name == course).FirstOrDefault();
            if (crs != null)
            {
                return View(new EditCourseViewModel(Environment)
                {
                    Faculty = facultyInfo,
                    Course = crs,
                    StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == course).OrderBy(x => x.Degree).ThenBy(x => x.Semester)
                });
            }
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult AddCourse(string NewCourseName)
        {
            if (!string.IsNullOrEmpty(NewCourseName))
            {
                NewCourseName = NewCourseName.Trim();
            }
            if (string.IsNullOrEmpty(NewCourseName))
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać kierunku studiów.", "Nazwa kierunku nie może być pusta.");
                return RedirectToAction("EventLog");
            }
            if (NewCourseName.Contains("<") || NewCourseName.Contains(">") || NewCourseName.Contains("|") || NewCourseName.Contains(":") || NewCourseName.Contains("?") || NewCourseName.Contains("*") || NewCourseName.Contains('"'))
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać kierunku studiów " + NewCourseName + ".", "Nazwa kierunku zawiera zakazane znaki.");
                return RedirectToAction("EventLog");
            }
            if (facultyInfo.Courses.Where(x => x.Name.ToLower() == NewCourseName.ToLower()).FirstOrDefault() != null)
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać kierunku studiów.", "Kierunek o tej nazwie już istnieje.");
                return RedirectToAction("EventLog");
            }
            else
            {
                Course crs = new Course(NewCourseName);
                facultyInfo.Courses = facultyInfo.Courses.Append(crs);
                facultyInfo.Serialize();

                string path = Path.Combine(Environment.WebRootPath, "files");
                string dir = Path.Combine(path, "subjectsDocs", NewCourseName);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                dir = Path.Combine(path, "studyPlans", NewCourseName);
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", NewCourseName);

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano nowy kierunek studiów: " + NewCourseName +  ".");

                return View("EditCourse", new EditCourseViewModel(Environment)
                {
                    Faculty = facultyInfo,
                    Course = crs,
                    StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == NewCourseName)
                });
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
                foreach (var v in timetables)
                {
                    studentsTimetableRepo.DeleteStudentsTimetable(v);
                }
                EventLogs.LogWarning(GetCurrentUserAsync().Result, "Usunięto wszystkie plany zajęć kierunku: " + courseName + ".");

                //usun przedmioty
                int[] subjects = subjectRepo.Subjects.Where(x => x.Course == course.Name).Select(x => x.Id).ToArray();
                foreach (var v in subjects)
                {
                    subjectRepo.DeleteSubject(v);
                }
                string dir = Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", course.Name);
                if (System.IO.Directory.Exists(dir))
                {
                    try
                    {
                        System.IO.Directory.Delete(dir, true);
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć kart przedmiotów kierunku: " + courseName + ".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
                EventLogs.LogWarning(GetCurrentUserAsync().Result, "Usunięto wszystkie przedmioty kierunku: " + courseName + ".");

                //usun plany studiow
                dir = Path.Combine(Environment.WebRootPath, "files", "studyPlans", course.Name);
                if (System.IO.Directory.Exists(dir))
                {
                    try
                    {
                        System.IO.Directory.Delete(dir, true);
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć planów studiów kierunku: " + courseName + ".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
                EventLogs.LogWarning(GetCurrentUserAsync().Result, "Usunięto wszystkie plany studiów kierunku: " + courseName + ".");

                //usun kierunek
                facultyInfo.Courses = facultyInfo.Courses.Where(x => x != course);
                facultyInfo.Serialize();

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto kierunek studiów: " + courseName + ".");
                return RedirectToAction("Index", "Admin", scroll);
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć kierunku: " + courseName + ".", "Kierunek o tej nazwie nie istnieje.");
                return RedirectToAction("EventLog");
            }
        }

        #region skladowe edycji
        [HttpPost]
        public IActionResult EditCourseName(string previousName, string newName)
        {
            if (!string.IsNullOrEmpty(newName))
            {
                newName = newName.Trim();
            }
            if (string.IsNullOrEmpty(newName))
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić nazwy kierunku " + previousName, "Nazwa kierunku nie może być pusta.");
                return RedirectToAction("EventLog");
            }
            if (newName.Contains("<") || newName.Contains(">") || newName.Contains("|") || newName.Contains(":") || newName.Contains("?") || newName.Contains("*") || newName.Contains('"'))
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić nazwy kierunku " + previousName, "Nowa nazwa kierunku zawiera zakazane znaki.");
                return RedirectToAction("EventLog");
            }

            Course courseP = facultyInfo.Courses.Where(x => x.Name == previousName).FirstOrDefault();
            Course courseN = facultyInfo.Courses.Where(x => x.Name == newName).FirstOrDefault();
            if (courseN != null)
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić nazwy kierunku z " + previousName + " na " + newName + ".", "Kierunek " + newName + " już istnieje.");
                return RedirectToAction("EventLog");
            }
            if (courseP != null)
            {

                var subjects = subjectRepo.Subjects.Where(x => x.Course == previousName).ToList();
                foreach(Subject sub in subjects)
                {
                    sub.Course = newName;
                    subjectRepo.EditSubject(sub);
                }
                var st = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == previousName).ToList();
                foreach(StudentsTimetable timetable in st)
                {
                    timetable.Course = newName;
                    studentsTimetableRepo.EditStudentsTimetable(timetable);
                }
                courseP.Name = newName;
                facultyInfo.Serialize();

                string path = Path.Combine(Environment.WebRootPath, "files");
                System.IO.Directory.Move(Path.Combine(path, "studyPlans", previousName), Path.Combine(path, "studyPlans", newName));
                System.IO.Directory.Move(Path.Combine(path, "subjectsDocs", previousName), Path.Combine(path, "subjectsDocs", newName));

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Zmieniono nazwę kierunku z " + previousName + " na " + newName + ".");

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", courseP.Name);

                return RedirectToAction("EditCourse", "Admin", routeValues);
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić nazwy kierunku " + previousName + " na " + newName + ".", "Kierunek nie istnieje.");
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
                    if (degree == 1)
                    {
                        if (course.FirstDegreeSpecialties.Contains(spec))
                        {
                            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać specjalizacji.", "Kierunek: " + courseName + ", Specjalizacja " + spec + " już istnieje.");
                            return RedirectToAction("EventLog");
                        }

                        course.FirstDegreeSpecialties.Add(spec);
                        facultyInfo.Serialize();

                        EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);
                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                    else if (degree == 2)
                    {
                        if (course.SecondDegreeSpecialties.Contains(spec))
                        {
                            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać specjalizacji.", "Kierunek: " + courseName + ", Specjalizacja " + spec + " już istnieje.");
                            return RedirectToAction("EventLog");
                        }

                        course.SecondDegreeSpecialties.Add(spec);
                        facultyInfo.Serialize();

                        EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);
                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                }
            }

            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać specjalizacji.", "Niepoprawne dane.");
            return RedirectToAction("EventLog");
        }

        [HttpPost]
        public IActionResult DeleteSpeciality(string courseName, string spec, int degree, string scroll)
        {
            Course course = facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault();
            if (course != null)
            {
                if (!string.IsNullOrEmpty(spec))
                {
                    if (degree == 1)
                    {
                        if (!course.FirstDegreeSpecialties.Contains(spec))
                        {
                            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można usunąć specjalizacji.", "Kierunek: " + courseName + ", Specjalizacja " + spec + " nie istnieje.");
                            return RedirectToAction("EventLog");
                        }

                        course.FirstDegreeSpecialties.Remove(spec);
                        facultyInfo.Serialize();

                        EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);
                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                    else if (degree == 2)
                    {
                        if (!course.SecondDegreeSpecialties.Contains(spec))
                        {
                            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można usunąć specjalizacji.", "Kierunek: " + courseName + ", Specjalizacja " + spec + " nie istnieje.");
                            return RedirectToAction("EventLog");
                        }

                        course.SecondDegreeSpecialties.Remove(spec);
                        facultyInfo.Serialize();

                        EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto specjalizację: " + spec + ".", "Kierunek: " + courseName + " Stopień: " + degree);

                        Dictionary<string, string> routeValues = new Dictionary<string, string>();
                        routeValues.Add("course", courseName);
                        return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                    }
                }
            }

            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można usunąć specjalizacji.", "Niepoprawne dane.");
            return RedirectToAction("EventLog");
        }

        [HttpPost]
        public IActionResult AddTimetable(StudentsTimetable studentsTimetable, string scroll)
        {
            if (ModelState.IsValid)
            {
                var timetables = studentsTimetableRepo.StudentsTimetables.ToList();
                foreach(StudentsTimetable time in timetables)
                {
                    if(time.Semester==studentsTimetable.Semester && time.Course==studentsTimetable.Course&&time.Degree==studentsTimetable.Degree)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać planu zajęć.", "Plan lekcji jest już przypipsany do tego semestru");
                        return RedirectToAction("EventLog");
                    }
                }

                if (studentsTimetable != null)
                {
                    studentsTimetableRepo.AddStudentsTimetable(studentsTimetable);

                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano plan zajęć. Kierunek: " + studentsTimetable.Course + ", Stopień: " + studentsTimetable.Degree + ", Semestr: " + studentsTimetable.Semester + ".");

                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", studentsTimetable.Course);
                    return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                }
                else
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać planu zajęć.", "Niepoprawne dane.");
                    return RedirectToAction("EventLog");
                }
            }

            return View("EditCourse", new EditCourseViewModel(Environment)
            {
                Faculty = facultyInfo,
                Course = facultyInfo.Courses.Where(x => x.Name == studentsTimetable.Course).FirstOrDefault(),
                StudentsTimetables = studentsTimetableRepo.StudentsTimetables.Where(x => x.Course == studentsTimetable.Course).OrderBy(x => x.Degree).ThenBy(x => x.Semester)
            });
        }

        [HttpPost]
        public IActionResult DeleteTimetable(string courseName, int studentsTimetableId, string scroll)
        {
            if (facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault() != null)
            {
                studentsTimetableRepo.DeleteStudentsTimetable(studentsTimetableId);

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto plan zajęć.", "Kierunek: " + courseName);

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", courseName);
                return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można usunąć planu zajęć.", "Niepoprawne dane.");
                return RedirectToAction("EventLog");
            }
        }

        [HttpPost]
        public IActionResult AddStudyPlan(string courseName, IFormFile file, string scroll)
        {
            if (facultyInfo.Courses.Where(x => x.Name == courseName).FirstOrDefault() != null)
            {
                if (Path.GetExtension(file.FileName) != ".pdf")
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać planu studiów (" + file.FileName + ") na kieruneku: " + courseName + ".", "Nieprawidłowy plik.");
                    return RedirectToAction("EventLog");
                }

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
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać planu studiów (" + file.FileName + ") na kieruneku: " + courseName + ".", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
                    }

                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano plan studiów (" + file.FileName + ").", "Kierunek: " + courseName);

                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", courseName);
                    return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                }
                else
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać planu studiów (" + file.FileName + ") na kieruneku: " + courseName + ".", "Plik już istnieje.");
                    return RedirectToAction("EventLog");
                }
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można dodać planu studiów (" + file.FileName + ") na kieruneku: " + courseName + ".", "Niepoprawne dane.");
                return RedirectToAction("EventLog");
            }
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
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć planu studiów (" + file + ") na kieruneku: " + courseName + ".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }

                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto plan studiów (" + file + ").", "Kierunek: " + courseName);

                    Dictionary<string, string> routeValues = new Dictionary<string, string>();
                    routeValues.Add("course", courseName);
                    return RedirectToAction("EditCourse", "Admin", routeValues, scroll);
                }
                else
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można usunąć planu studiów (" + file + ") na kieruneku: " + courseName + ".", "Plik nie istnieje.");
                    return RedirectToAction("EventLog");
                }
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie można usunąć planu studiów (" + file + ") na kieruneku: " + courseName + ".", "Niepoprawne dane.");
                return RedirectToAction("EventLog");
            }
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
            Article article = articlesRepo.Articles.FirstOrDefault(x => x.Id == id);
            if (article is null)
            {
                return NotFound();
            }
            else
            {
                return View(new EditArticleViewModel() { Faculty = facultyInfo, Article = article });
            }
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
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać/edytować aktualności.", "Ochrona przed atakiem XSS.");
                    return RedirectToAction("EventLog");
                }

                string path = Path.Combine(Environment.WebRootPath, "pictures", "picsArticle");
                string dir = Path.Combine(path, article.Id.ToString());
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                if (photoIn != null)
                {
                    if (!string.IsNullOrEmpty(article.PhotoIn) && System.IO.File.Exists(Path.Combine(dir, article.PhotoIn)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(dir, article.PhotoIn));
                        }
                        catch (Exception)
                        {
                            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia aktualności.", "Błąd serwera.");
                        }
                    try
                    {
                        using (FileStream fs = new FileStream(Path.Combine(dir, photoIn.FileName), FileMode.Create))
                        {
                            photoIn.CopyTo(fs);
                        }
                        article.PhotoIn = photoIn.FileName;
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia aktualności.", "Problem z plikiem " + photoIn.FileName);
                    }    
                }

                if (photoCover != null)
                {
                    if (!string.IsNullOrEmpty(article.PhotoCover) && System.IO.File.Exists(Path.Combine(dir, article.PhotoCover)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(dir, article.PhotoCover));
                        }
                        catch (Exception)
                        {
                            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia aktualności.", "Błąd serwera.");
                        }
                    try
                    {
                        using (FileStream fs = new FileStream(Path.Combine(dir, photoCover.FileName), FileMode.Create))
                        {
                            photoCover.CopyTo(fs);
                        }
                        article.PhotoCover = photoCover.FileName;
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia aktualności.", "Problem z plikiem " + photoCover.FileName);
                    }
                }

                if (article.Id == 0)
                {
                    articlesRepo.AddArticle(article);
                    int? id = articlesRepo.Articles.Where(x => x.Title == article.Title && x.DateTime == article.DateTime).Select(x => x.Id).FirstOrDefault();
                    if (id != null)
                    {
                        System.IO.Directory.Move(Path.Combine(path, 0.ToString()), Path.Combine(path, id.ToString()));
                    }
                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano nową aktualność.", article.Title);
                }
                else
                {
                    articlesRepo.EditArticle(article);
                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Edytowano aktualność.", article.Title);
                }

                return RedirectToAction("ArticleList");
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać/edytować aktualności.", "Niepoprawne dane.");
                return RedirectToAction("EventLog");
            }
        }

        [HttpPost]
        public IActionResult DeleteArticle(int id)
        {
            Article article = articlesRepo.Articles.Where(x => x.Id == id).FirstOrDefault();
            if (article != null)
            {
                string path = Path.Combine(Environment.WebRootPath, "pictures", "picsArticle", id.ToString());
                if (System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.Delete(path, true);
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć aktualności: " + article.Title + ".", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }

                articlesRepo.DeleteArticle(id);

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto aktualność.", article.Title);

                return RedirectToAction("ArticleList");
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć aktualności: " + article.Title + ".", "Niepoprawne dane.");
                return RedirectToAction("EventLog");
            }
        }
        #endregion

        #region Employee
        [HttpGet]
        public IActionResult EmployeeList(int page = 1, string name = "", string surname = "")
        {
            IEnumerable<Employee> employees = employeesRepo.Employees;
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

            employees = employees.OrderBy(x => x.LastName).ThenBy(x => x.Name).Skip((page - 1) * PageSize).Take(PageSize);

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
            Employee employee = employeesRepo.Employees.FirstOrDefault(x => x.Id == id);
            if (employee is null)
            {
                return NotFound();
            }
            else
            {
                return View(new EditEmployeeViewModel() { Faculty = facultyInfo, Employee = employee});
            }
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
                string dir = Path.Combine(path, employee.Id.ToString());
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                if (photo != null)
                {
                    if (!string.IsNullOrEmpty(employee.Photo) && System.IO.File.Exists(Path.Combine(dir, employee.Photo)))
                        try
                        {
                            System.IO.File.Delete(Path.Combine(dir, employee.Photo));
                        }
                        catch (Exception) 
                        {
                            EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia pracownika (" + employee.Name +" " + employee.LastName+").", "Błąd serwera.");
                        }
                    try
                    {
                        using (FileStream fs = new FileStream(Path.Combine(dir, photo.FileName), FileMode.Create))
                        {
                            photo.CopyTo(fs);
                        }
                        employee.Photo = photo.FileName;
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać zdjęcia pracownika (" + employee.Name + " " + employee.LastName + ").", "Problem z plikiem " + photo.FileName);
                    }
                }

                if (employee.Id == 0)
                {
                    employeesRepo.AddEmployee(employee);
                    int? id = employeesRepo.Employees.Where(x => x.Name == employee.Name && x.LastName == employee.LastName && x.Mail == employee.Mail).Select(x => x.Id).FirstOrDefault();
                    if (id != null)
                    {
                        System.IO.Directory.Move(Path.Combine(path, 0.ToString()), Path.Combine(path, id.ToString()));
                    }

                    if (string.IsNullOrEmpty(employee.Photo))
                        EventLogs.LogWarning(GetCurrentUserAsync().Result, "Dodano nowego pracownika (" + employee.Name + " " + employee.LastName + ").", "Brak zdjęcia.");
                    else
                        EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano nowego pracownika (" + employee.Name + " " + employee.LastName + ").");
                }
                else
                {
                    employeesRepo.EditEmployee(employee);
                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Edytowano pracownika (" + employee.Name + " " + employee.LastName + ").");
                }

                return RedirectToAction("EmployeeList");
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się dodać/edytować pracownika (" + employee.Name + " " + employee.LastName + ").", "Niepoprawne dane.");
                return RedirectToAction("EventLog");
            }
        }

        [HttpPost]
        public IActionResult DeleteEmployee(int id)
        {
            Employee employee = employeesRepo.Employees.Where(x => x.Id == id).FirstOrDefault();
            if (employee != null)
            {
                string path = Path.Combine(Environment.WebRootPath, "pictures", "picsEmployee", id.ToString());
                if (System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.Delete(path, true);
                    }
                    catch (Exception) 
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć pracownika (" + employee.Name + " " + employee.LastName + ").", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
            }
            employeesRepo.DeleteEmployee(id);

            EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto pracownika pracownika (" + employee.Name + " " + employee.LastName + ").");

            return RedirectToAction("EmployeeList");
        }
        #endregion

        #region Subject
        [HttpGet]
        public IActionResult SubjectsList(string course)
        {
            IEnumerable<Subject> subs = subjectRepo.Subjects.Where(x => x.Course == course).OrderBy(x => x.Name);
            if (facultyInfo.ExistsCourse(course))
            {
                return View(new SubjectsListViewModel() { Faculty = facultyInfo, Subjects = subs, CourseName = course });
            }
            else
                return NotFound();
        }
        [HttpGet]
        public IActionResult EditSubject(int subjectId)
        {
            Subject subject = subjectRepo.Subjects.Where(x => x.Id == subjectId).FirstOrDefault();
            if (subject is null)
                return NotFound();
            else
                return View(new EditSubjectViewModel() { Faculty = facultyInfo, Subject = subject });
        }

        public IActionResult AddSubject(string course)
        {
            return View("EditSubject", new EditSubjectViewModel() { Faculty = facultyInfo, Subject = new Subject() { Course = course } });
        }

        [HttpPost]
        public IActionResult EditSubject(Subject subject, IFormFile doc = null)
        {
            if (ModelState.IsValid && (!string.IsNullOrEmpty(subject.File) || doc != null))
            {
                if (subject is null)
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się edytować/dodać przedmiotu (" + subject.Name + ").", "Przedmiot nie istnieje.");
                    return RedirectToAction("EventLog");
                }

                string path = Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", subject.Course);
                if (doc != null)
                {
                    string dir = Path.Combine(path, subject.Id.ToString());
                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);

                    if (System.IO.File.Exists(Path.Combine(dir, subject.File)))
                    {
                        try
                        {
                            System.IO.File.Delete(Path.Combine(dir, subject.File));
                        }
                        catch (Exception)
                        {
                            EventLogs.LogWarning(GetCurrentUserAsync().Result, "Nie udało się usunąć poprzedniego pliku z kartą przedmiotu (" + subject.Name + ").", "Błąd serwera.");
                        }
                    }

                    try
                    {
                        using (FileStream fs = new FileStream(Path.Combine(dir, doc.FileName), FileMode.Create))
                        {
                            doc.CopyTo(fs);
                        }
                        subject.File = doc.FileName;
                    }
                    catch (Exception)
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się edytować/dodać przedmiotu (" + subject.Name + ").", "Problem z plikiem.");
                        return RedirectToAction("EventLog");
                    }
                }

                if (subject.Id == 0)
                {
                    subjectRepo.AddSubject(subject);
                    int? id = subjectRepo.Subjects.ToList().LastOrDefault().Id;
                    if (id != null)
                    {
                        System.IO.Directory.Move(Path.Combine(path, 0.ToString()), Path.Combine(path, id.ToString()));
                    }
                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Dodano nowy przedmiot (" + subject.Name + ").", "Kierunek: " + subject.Course);
                }
                else
                {
                    subjectRepo.EditSubject(subject);
                    EventLogs.LogInformation(GetCurrentUserAsync().Result, "Edytowano przedmiot (" + subject.Name + ").", "Kierunek: " + subject.Course);
                }

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", subject.Course);
                return RedirectToAction("SubjectsList", "Admin", routeValues);
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się edytować/dodać przedmiotu (" + subject.Name + ").", "Nieprawidłowe dane.");
                return RedirectToAction("EventLog");
            }
        }

        [HttpPost]
        public IActionResult DeleteSubject(int subjectId)
        {
            Subject subject = subjectRepo.Subjects.Where(x => x.Id == subjectId).FirstOrDefault();
            if (subject != null)
            {
                string path = Path.Combine(Environment.WebRootPath, "files", "subjectsDocs", subject.Course, subjectId.ToString());
                if (System.IO.Directory.Exists(path))
                {
                    try
                    {
                        System.IO.Directory.Delete(path, true);
                    }
                    catch (Exception) 
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć przedmiotu (" + subject.Name + ").", "Błąd serwera.");
                        return RedirectToAction("EventLog");
                    }
                }
                subjectRepo.DeleteSubject(subjectId);

                EventLogs.LogInformation(GetCurrentUserAsync().Result, "Usunięto przedmiot (" + subject.Name + ").", "Kierunek: " + subject.Course);

                Dictionary<string, string> routeValues = new Dictionary<string, string>();
                routeValues.Add("course", subject.Course);
                return RedirectToAction("SubjectsList", "Admin", routeValues);
            }
            else
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć przedmiotu (" + subject.Name + ").", "Przedmiot nie istnieje");
                return RedirectToAction("EventLog");
            }
        }
        #endregion

        #region Identity
        [HttpPost]
        public async Task <IActionResult> CreateUser(User user, string confirmPassword)
        {
            if (String.IsNullOrWhiteSpace(user.Password))
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się utworzyć nowego użytkownika, hasło nie może posiadać białych znaków.", user.Name);
                return RedirectToAction("EventLog");
            }
            else if (ModelState.IsValid)
            {
                if (user.Password.Length > 32)
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się utworzyć nowego użytkownika.", "Hasło jest za długie. " + user.Name);
                    return RedirectToAction("EventLog");
                }
                else if (user.Password != confirmPassword)
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się utworzyć nowego użytkownika, hasła nie są takie same.", user.Name);
                    return RedirectToAction("EventLog");
                }
                else if (user.Password.Contains(" "))
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się utworzyć nowego użytkownika, hasło nie może posiadać białych znaków.", user.Name);
                    return RedirectToAction("EventLog");
                }
                else if(user.Password is null)
                {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się utworzyć nowego użytkownika, hasło nie może posiadać białych znaków.", user.Name);
                    return RedirectToAction("EventLog");
                }
                else
                {
                    IdentityUser appUser = new IdentityUser { UserName = user.Name };
                    IdentityResult result = await userManager.CreateAsync(appUser, user.Password);
                    if (result.Succeeded)
                    {
                        EventLogs.LogInformation(GetCurrentUserAsync().Result, "Utworzono nowego użytkownika.", user.Name);
                        return RedirectToAction("AccountsList");
                    }
                    else
                    {
                        foreach (IdentityError e in result.Errors)
                        {
                            ModelState.AddModelError("", e.Description);
                        }
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się utworzyć nowego użytkownika. Hasło nie spełnia wymagań", user.Name);
                        return RedirectToAction("EventLog");
                    }
                }
                
            }

            return RedirectToAction("AccountsList");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            IdentityUser currentUser = await GetCurrentUserAsync();
            IdentityUser user = null;
            if (!string.IsNullOrEmpty(id))
            {
                user = userManager.FindByIdAsync(id).Result;
            }
            if (user != null)
            {
                if (user == currentUser)
                {
                    EventLogs.LogWarning(GetCurrentUserAsync().Result, "Nie można usunąć użytkownika.", "Użytkownik jest obecnie zalogowany na tym koncie");
                    return RedirectToAction("EventLog");
                }
                else
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        EventLogs.LogWarning(GetCurrentUserAsync().Result, "Pomyślnie usunięto użytkownika użytkownika. ", user.UserName);
                        return RedirectToAction("AccountsList");
                    }
                    else
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć użytkownika. ", user.UserName);
                        return RedirectToAction("EventLog");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Nie znaleziono użytkownika");

                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się usunąć użytkownika.", "Nie znaleziono użytkownika: " + user.UserName);
                return RedirectToAction("EventLog");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUserPassword(string id, string oldPassword, string newPassword, string newPassword2)
        {
            IdentityUser user = await GetCurrentUserAsync();
            if (newPassword!=newPassword2)
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić hasła.", "Hasła nie są takie same.");
                return RedirectToAction("EventLog");
            }
            else if(oldPassword==null)
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić hasła.", "Nie wprowadzono starego hasła.");
                return RedirectToAction("EventLog");
            }
            else if(oldPassword==newPassword)
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić hasła.", "Nowe hasło nie może być takie same jak stare.");
                return RedirectToAction("EventLog");
            }
            else if(newPassword==null)
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić hasła.", "Nie wprowadzono nowego hasła.");
                return RedirectToAction("EventLog");
            }
            else if(newPassword.Length>32)
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić hasła.", "Wprowadzone nowe hasło jest za długie.");
                return RedirectToAction("EventLog");
            }
            else if (newPassword.Contains(" ") || String.IsNullOrWhiteSpace(newPassword))
            {
                EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się utworzyć nowego użytkownika.", "Hasło nie może posiadać białych znaków.");
                return RedirectToAction("EventLog");
            }
            else if(user==null)
            {
                    EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić hasła.", "Nie znaleziono użytkownika: "+user.UserName);
                    return RedirectToAction("EventLog");
            }
            else
            {
                if (newPassword != null)
                {
                    IdentityResult result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                    if (result.Succeeded)
                    {
                        EventLogs.LogWarning(GetCurrentUserAsync().Result, "Pomyślna zmiana hasła.", "");

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        EventLogs.LogError(GetCurrentUserAsync().Result, "Nie udało się zmienić hasła.", "Stare hasło nie zostało wprowadzone poprawnie, lub nowe hasło nie spełnia wymagań.");
                        return RedirectToAction("EventLog");
                    }
                }
            }
            return RedirectToAction("EventLog");


        }


        private Task<IdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        #endregion

        public IActionResult EventLog(int page = 1)
        {
            IEnumerable<Models.EventLog.Log> logs = Models.EventLog.EventLogs.Logs;
            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = logs.Count()
            };
            logs = logs.Skip((page - 1) * PageSize).Take(PageSize);

            return View(new EventLogViewModel() { Faculty = facultyInfo, Logs = logs, PagingInfo = pagingInfo });
        }

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


        //validator XSS
        private bool XSSValidate(string input)
        {
            input = System.Web.HttpUtility.HtmlDecode(input);
            string[] legalMarkups = new string[] {"p", "br", "h2", "h3", "h4", "strong", "i", "u", "span", "ul", "li", "ol", "a", "figure", "table", "tbody", "tr", "td"};

            MatchCollection matches = Regex.Matches(input, "<.*?>");

            foreach (var m in matches)
            {
                bool result = false; 
                foreach (var v in legalMarkups)
                {
                    if (Regex.IsMatch(m.ToString(), $"<{v}.*?>") || Regex.IsMatch(m.ToString(), $"</{v}>"))
                    {
                        result = true;
                        break;
                    }
                }
                if (!result)
                    return result;
            }

            return true;
        }
    }
}
