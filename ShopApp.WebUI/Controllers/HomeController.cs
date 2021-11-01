using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Models;
using ShopApp.WebUI.ViewModels;
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
            var products = new List<Product>()
            {
                new Product{Name="Nokia",Price=50,Description="Nothing",IsApproved=true},
                new Product{Name="Iphone 5",Price=5000,Description="Good",IsApproved=false},
                new Product{Name="Iphone 6",Price=5600,Description="Good",IsApproved=true},
                new Product{Name="Iphone 7",Price=5546,Description="Good"},
                new Product{Name="Iphone 8",Price=4646,Description="Good",IsApproved=true},
                new Product{Name="Iphone 9",Price=3554,Description="Good"},
                new Product{Name="Iphone 11",Price=978568,Description="Good",IsApproved=true},
            };

            var productViewModel = new ProductViewModel()
            {
                Products = products
            };

            return View(productViewModel);
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
