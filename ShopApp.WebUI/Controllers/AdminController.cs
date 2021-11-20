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
        private ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        #region Product
        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products=_productService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreate(ProductModel productModel)
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
        public IActionResult ProductEdit(int? id)
        {
            if(id==null)
            {
                return NotFound(); 
            }
            var entity = _productService.GetByIdWithCategories((int)id);

            if(entity==null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                Description = entity.Description,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public IActionResult ProductEdit(ProductModel productModel)
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
        [HttpPost]
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
        #endregion

        #region Category
        public IActionResult CategoryList()
        {
            return View(new CategoryListViewModel()
            {
                Categories = _categoryService.GetAll()
            });
        }

        [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel categoryModel)
        {
            var entity = new Category()
            {
                Name = categoryModel.Name,
                Url=categoryModel.Url       
            };
            _categoryService.Create(entity);

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı kateqoriya uğurla əlavə edildi",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            return RedirectToAction("CategoryList");
        }

        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _categoryService.GetByIdWithProducts((int)id);

            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel categoryModel)
        {
            var entity = _categoryService.GetById(categoryModel.CategoryId);
            if (entity == null)
            {
                return NotFound();
            }
            else
            {
                entity.Name = categoryModel.Name;
                entity.Url = categoryModel.Url;
               

                _categoryService.Update(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı kateqoriya uğurla yeniləndi",
                AlertType = "success"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }

            var msg = new AlertMessage()
            {
                Message = $"{entity.Name} adlı kateqoriya uğurla silindi",
                AlertType = "danger"
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int productId,int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);

            return Redirect("/admin/categories/" + categoryId);
        }
        #endregion
    }
}
