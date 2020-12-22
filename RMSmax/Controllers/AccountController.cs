﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RMSmax.Models.ViewModels;
using RMSmax.Models;
using Microsoft.AspNetCore.Hosting;

namespace RMSmax.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private Faculty facultyInfo;
        public AccountController(UserManager<IdentityUser> user, SignInManager<IdentityUser> signIn, IWebHostEnvironment env)
        {
            facultyInfo = Faculty.FacultyInstance is null ? new Faculty(env.WebRootPath) : Faculty.FacultyInstance;
            userManager = user;
            signInManager = signIn;
        }
        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel { ReturnUrl = returnUrl, Faculty = facultyInfo});
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if(ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Name);
                if(user!=null)
                {
                    await signInManager.SignOutAsync();
                    if((await signInManager.PasswordSignInAsync(user,loginModel.Password,false,false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Admin/Index");
                    }
                }
            }
            ModelState.AddModelError("", "Nieprawidłowa nazwa lub hasło");
            loginModel.Faculty = facultyInfo;
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl="/")
        {

            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }


    }
}