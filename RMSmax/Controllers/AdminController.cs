﻿using Microsoft.AspNetCore.Mvc;
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
            return View(new IndexViewModel() { FacultyCourses = facultyInfo.Courses, Faculty = facultyInfo });
        }

        [HttpPost]
        public IActionResult EditFacultyInfo(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                facultyInfo.Update(faculty);
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", new IndexViewModel() { FacultyCourses = facultyInfo.Courses, Faculty = faculty });
            }
        }

        [HttpGet]
        public IActionResult EditCourse()
        {
            return View(new MainViewModel() { FacultyCourses = facultyInfo.Courses });
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            return View("EditCourse", new MainViewModel() { FacultyCourses = facultyInfo.Courses });
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
            return View();
        }

        [HttpGet]
        public IActionResult EditArticle()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddArticle()
        {
            return View("EditArticle");
        }

        [HttpPost]
        public IActionResult EditArticle(Article article)
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteArticle(Article article)
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmployeeList()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EditEmployee()
        {
            return View();
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
    }
}
