using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopApp.WebUI.Data;
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
            return View();
        }

        public IActionResult List(int? id,string q,double? min_price,double? max_price)
        {
            var products = ProductRepository.Products;
            if(id!=null)
            {
                products = products.Where(p => p.CategoryId == id).ToList();
            }

            if(!string.IsNullOrEmpty(q))
            {
                products = products.Where(i => i.Name.Contains(q)||i.Description.Contains(q)).ToList();
            }

            var productViewModel = new ProductViewModel()
            {
                Products = products
            };

            return View(productViewModel);
        }

        [HttpGet]
        public IActionResult Details(int id)
        { 
            return View(ProductRepository.GetProductById(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories =new SelectList(CategoryRepository.Categories,"CategoryId","Name");

            return View(new Product());
        }
        [HttpPost]
        public IActionResult Create(Product p)
        {
            if(ModelState.IsValid)
            {
                ProductRepository.AddProduct(p);

                return RedirectToAction("list");
            }

            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");

            return View(p);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories, "CategoryId", "Name");

            return View(ProductRepository.GetProductById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product p)
        {
            ProductRepository.EditProduct(p);
            return RedirectToAction("list");
        }

        [HttpPost]
        public IActionResult Delete(int ProductId)
        {
            ProductRepository.DeleteProduct(ProductId);
            return RedirectToAction("list");
        }
    }
}
