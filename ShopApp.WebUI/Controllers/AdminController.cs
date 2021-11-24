using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Helpers;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Controllers
{
   [Authorize]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;
        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        #region Roles

        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            var members = new List<User>();
            var nonmembers = new List<User>();

            foreach (var user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)?members:nonmembers;
                list.Add(user);
            }
            var model = new RoleDetails()
            {
                Role=role,
                Members=members,
                NonMembers=nonmembers
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel roleEditModel)
        {
            if(ModelState.IsValid)
            {
                foreach(var userId in roleEditModel.IdsToAdd ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);

                    if(user!=null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, roleEditModel.RoleName);
                        if(!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }    
                }


                foreach (var userId in roleEditModel.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, roleEditModel.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }

            return Redirect("/admin/role/" + roleEditModel.RoleId);
        }
        
        public IActionResult RoleList()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult RoleCreate()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel roleModel)
        {
            if(ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleModel.Name));

                if(result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }
            }

            return View(roleModel);
        }
        #endregion

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
            if(ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = productModel.Name,
                    Url = productModel.Url,
                    Price = productModel.Price,
                    Description = productModel.Description,
                    ImageUrl = productModel.ImageUrl
                };

               if( _productService.Create(entity))
                {
                    CreateMessage(UiMessages.CreateSuccesMessage, "success");
                    return RedirectToAction("ProductList");
                }
               else
                {
                    CreateMessage(_productService.ErrorMessage, "danger");
                    return View(productModel);
                }              
            }
            else
            {
                return View(productModel);
            }
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
                IsApproved=entity.IsApproved,
                IsHome=entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task< IActionResult> ProductEdit(ProductModel productModel,int[] categoryIds,IFormFile file)
        {
            if(ModelState.IsValid)
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
                    entity.Price = productModel.Price;
                    entity.Description = productModel.Description;
                    entity.IsApproved = productModel.IsApproved;
                    entity.IsHome = productModel.IsHome;

                    if(file!=null)
                    {
                        var extention = Path.GetExtension(file.FileName);
                        var randomName = string.Format($"{Guid.NewGuid()}{extention}");
                        entity.ImageUrl = randomName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img",randomName);

                        using(var stream=new FileStream(path,FileMode.Create))
                        {
                           await file.CopyToAsync(stream);
                        }
                    }

                    if (_productService.Update(entity, categoryIds))
                    {
                        CreateMessage(UiMessages.UpdateSuccesMessage, "success");
                        return RedirectToAction("ProductList");
                    }
                    else
                    {
                        CreateMessage(_productService.ErrorMessage, "danger");
                    }
                }              
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(productModel);
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
            if(ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = categoryModel.Name,
                    Url = categoryModel.Url
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

            return View(categoryModel);
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
            if(ModelState.IsValid)
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

            return View(categoryModel);
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

        #region Private Methods
        private void CreateMessage(string message,string alerttype)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
        }
        #endregion
    }
}
