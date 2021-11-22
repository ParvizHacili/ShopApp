using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Register(RegisterModel registerModel)
        {
            if(!ModelState.IsValid)
            {
                return View(registerModel);
            }

            var user = new User()
            {
                FirstName = registerModel.FirstName,
                LastName=registerModel.LastName,
                UserName=registerModel.UserName,
                Email=registerModel.Email
            };
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("Password", "Bilinməyən xəta baş verdi zəhmət olmasa yenidən cəhd edin");
            return View(registerModel);
        }
    }
}
