using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace RMSmax.Models.ViewModels
{
    public class StudiesViewModel : MainViewModel
    {
        private string rootPath;
        public Course Course { get; set; }
        public IEnumerable<StudentsTimetable> StudentsTimetables { get; set; }
        public IEnumerable<Subject> Subjects { get; set; }
        public IList<string> StudyPlans
        {
            get
            {
                List<string> files = new List<string>();
                string path = Path.Combine(rootPath, "files", "studyPlans", Course.Name);
                foreach (var file in new DirectoryInfo(path).GetFiles())
                {
                    files.Add(file.Name);
                }

                return files;
            }
        }
        public StudiesViewModel(IWebHostEnvironment env)
        {
            rootPath = env.WebRootPath;
        }
    }
}
