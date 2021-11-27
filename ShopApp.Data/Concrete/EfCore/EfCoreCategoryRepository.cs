using ShopApp.Data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ShopApp.Data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public void DeleteFromCategory(int productId, int categoryId)
        {

            using (var context = new ShopContext())
            {
                var cmd = $"Delete from ProductCategory where ProductId=@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd, productId, categoryId);
            }
        }

        public Category GetByIdWithProducts(int CategororyId)
        {
           using(var context=new ShopContext())
            {
                return context.Categories.Where(i => i.CategoryId == CategororyId)
                    .Include(i => i.ProductCategories).ThenInclude(i => i.Product).FirstOrDefault();
            }
        }

      
    }
}
