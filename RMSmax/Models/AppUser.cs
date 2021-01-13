using Microsoft.AspNetCore.Identity;


namespace RMSmax.Models
{
    public class AppUser:IdentityUser
    {
        public AppUser()
        {
        }
        public AppUser(string login):base(login)
        {
            
        }
    }
}
