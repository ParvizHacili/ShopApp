using Microsoft.AspNetCore.Mvc;
using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.ViewComponents
{
    public class CategoriesViewComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var categories = new List<Category>()
            {
                new Category { Name = "Telefon", Description = "Telefon Kateqoriyasi" },
                new Category { Name = "Kompyuter", Description = "Kompyuter Kateqoriyasi" },
                new Category { Name = "Elektronika", Description = "Elektronika Kateqoriyasi" }
            };

            return View(categories);
        }
    }
}
