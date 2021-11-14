using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;
        public AdminController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products=_productService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel productModel)
        {
            var entity = new Product()
            {
                Name = productModel.Name,
                Url = productModel.Url,
                Price=productModel.Price,
                Description=productModel.Description,
                ImageUrl=productModel.ImageUrl
            };
            _productService.Create(entity);
            return RedirectToAction("ProductList");
        }
    }
}
