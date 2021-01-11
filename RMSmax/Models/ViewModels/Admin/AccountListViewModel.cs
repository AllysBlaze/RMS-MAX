using System.Collections;
using System.Linq;

namespace RMSmax.Models.ViewModels.Admin
{
    public class AccountListViewModel : MainViewModel
    {
        public User User { get; set; }
        public string ConfirmPassword { get; set; }
        public IQueryable<AppUser> UserList { get; set; }
    }
}
