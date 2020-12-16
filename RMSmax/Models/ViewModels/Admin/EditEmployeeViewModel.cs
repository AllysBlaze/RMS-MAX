using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EditEmployeeViewModel : MainViewModel
    {
        public Employee Employee { get; set; }
        public IFormFile Photo { set; get; }
    }
}
