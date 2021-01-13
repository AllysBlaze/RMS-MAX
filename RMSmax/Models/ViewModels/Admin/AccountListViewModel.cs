using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace RMSmax.Models.ViewModels.Admin
{
    public class AccountListViewModel : MainViewModel
    {
        public User User { get; set; } 
        public string ConfirmPassword { get; set; }
        public IQueryable<IdentityUser> UserList { get; set; }
    }
}
