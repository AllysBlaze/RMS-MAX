using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EditCourseViewModel : MainViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<StudentsTimetable> StudentsTimetables { get; set; }
        private string rootPath;
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
        public EditCourseViewModel(IWebHostEnvironment env)
        {
            rootPath = env.WebRootPath;
        }
    }
}
