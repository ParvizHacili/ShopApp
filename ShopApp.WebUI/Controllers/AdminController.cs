using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

            var msg= new AlertMessage()
            {
                Message = $"{entity.Name} adlı məhsul uğurla əlavə edildi",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("ProductList");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound(); 
            }
            var entity = _productService.GetByID((int)id);

            if(entity==null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId=entity.ProductId,
                Name=entity.Name,
                Url=entity.Url,
                ImageUrl=entity.ImageUrl,
                Price=entity.Price,
                Description=entity.Description
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ProductModel productModel)
        {
            var entity = _productService.GetByID(productModel.ProductId);
            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.Name = productModel.Name;
                entity.Url = productModel.Url;
                entity.ImageUrl = productModel.ImageUrl;
                entity.Price = productModel.Price;
                entity.Description = productModel.Description;

                _productService.Update(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı məhsul uğurla yeniləndi",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }

        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetByID(productId);
            if(entity!=null)
            {
                _productService.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı məhsul uğurla silindi",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("ProductList");
        }
    }
}
