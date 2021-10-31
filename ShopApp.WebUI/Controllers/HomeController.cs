using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //https://localhost:44318/

        public IActionResult Index()
        {
            int time= DateTime.Now.Hour;

            ViewBag.Greeting = time > 12 ? "Iyi Gunler" : "Gunaydin";
            ViewBag.UserName = "Parviz";
            return View();
        }


        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View("MyView");
        }
    }
}
