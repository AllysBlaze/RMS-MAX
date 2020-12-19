using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EditSubjectViewModel : MainViewModel
    {
        public Subject Subject { get; set; }
        public IFormFile Doc { get; set; }
    }
}
