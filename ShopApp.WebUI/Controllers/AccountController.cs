using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.WebUI.EmailServices;
using ShopApp.WebUI.Helpers;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public IActionResult Login(string ReturnUrl=null)
        {

            return View(new LoginModel
            {
                ReturnUrl = ReturnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Login(LoginModel loginModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginModel);
            }

            //var user = await _userManager.FindByNameAsync(loginModel.UserName);
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if(user==null)
            {
                ModelState.AddModelError("", "İstifadəçi adı mövcud deyil");
                return View(loginModel);
            }

            if(! await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Zəhmət olmasa email adresinizə gələn link ilə hesabınızı təsdiqləyin");

                return View(loginModel);
            }


            var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if(result.Succeeded)
            {
                return Redirect(loginModel.ReturnUrl??"~/");
            }
            ModelState.AddModelError("", "Istifadəçi adı və ya şifrə yanlışdır");
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail","Account",new { 
                    userId=user.Id,
                    token=code
                });

                await _emailSender.SendEmailAsync(registerModel.Email, "Hesab Doğrulaması",$"Zəhmət olmasa hesabınızı təsdiqləmək üçün linkə <a href='https://localhost:44318{url}'>tıklayın!</a>");

                return RedirectToAction("Login", "Account");
            }

            ModelState.AddModelError("Password", "Bilinməyən xəta baş verdi zəhmət olmasa yenidən cəhd edin");
            return View(registerModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("~/");
        }

        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            if(userId==null || token==null)
            {
                CreateMessage("Invalid Token", "danger");
                return View();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if(user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    CreateMessage("Hesabınız Təsdiqləndi", "success");                
                    return View();
                }
            }
            CreateMessage("Hesabınız Təsdiqlənmədi", "warning");
            return View();
        }

        private void CreateMessage(string message, string alerttype)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
    }
}
