using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Models;
using ShopApp.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            var product = new Product { Name = "Xiaomi", Price = 300, Description = "Awesome" };

            //ViewData["Category"] = "Telefonlar";
            //ViewData["Product"] = product;

            //ViewBag.Category = "Telefonlar";
            //ViewBag.Product = product;


            return View(product);
        }

        public IActionResult List()
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

            var category = new Category { Name = "Telefonlar", Description = "Telefon Categorsi" };

            var productViewModel = new ProductViewModel()
            {
                Category = category,
                Products = products
            };

            return View(productViewModel);
        }

        public IActionResult Details(int id)
        {
            //ViewBag.Name = "Samsung Galaxy";
            //ViewBag.Price = 500;
            //ViewBag.Description = "good telephone";

            var product = new Product();
            product.Name = "Samsung Galaxy";
            product.Price = 500;
            product.Description = "Good";

            return View(product);
        }
    }
}
