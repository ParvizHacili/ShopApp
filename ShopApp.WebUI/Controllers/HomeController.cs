using Microsoft.AspNetCore.Mvc;
using ShopApp.Data.Abstract;
using ShopApp.WebUI.ViewModels;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShopApp.Business.Abstract;

namespace ShopApp.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //https://localhost:44318/

        private IProductService _productService;
        public HomeController(IProductService productService)
        {
            this._productService = productService;
        }

        public IActionResult Index()
        {
            var productViewModel = new ProductListViewModel()
            {
                Products = _productService.GetHomePageProducts()
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
