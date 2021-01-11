using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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
