using Microsoft.AspNetCore.Http;

namespace RMSmax.Models.ViewModels.Admin
{
    public class EditSubjectViewModel : MainViewModel
    {
        public Subject Subject { get; set; }
        public IFormFile Doc { get; set; }
    }
}
