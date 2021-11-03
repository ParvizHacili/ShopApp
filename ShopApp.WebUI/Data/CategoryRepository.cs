﻿using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Data
{
    public class CategoryRepository
    {
        private static List<Category> _categories = null;

        static CategoryRepository()
        {
            _categories = new List<Category>
            {
                new Category {CategoryId=1, Name = "Telefon", Description = "Telefon Kateqoriyasi" },
                new Category {CategoryId=2,Name = "Kompyuter", Description = "Kompyuter Kateqoriyasi" },
                new Category {CategoryId=3,Name = "Elektronika", Description = "Elektronika Kateqoriyasi" },
                new Category {CategoryId=4,Name = "Kitab", Description = "Kitab Kateqoriyasi" }
            };
        }

        public static List<Category> Categories
        {
            get
            {
                return _categories;
            }
        }

        public static void AddCategory(Category category)
        {
            _categories.Add(category);
        }

        public static Category GetCategoryById(int id)
        {
            return _categories.FirstOrDefault(c => c.CategoryId == id);
        }

    }
}
