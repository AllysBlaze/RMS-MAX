﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace RMSmax.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPass = "Secret123$"; 
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<IdentityUser> userManager = app.ApplicationServices.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser user = await userManager.FindByIdAsync(adminUser);
            if(user==null)
            {
                user = new IdentityUser("Admin");
                await userManager.CreateAsync(user, adminPass);
            }
        }
    }
}
