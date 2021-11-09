using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.Data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public List<Product> GetPopularProducts()
        {
           using(var context=new ShopContext())
            {
                return context.Products.ToList();
            }
        }

        public Product GetProductDetails(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(i => i.ProductId == id).Include(i => i.ProductCategories).ThenInclude(i => i.Category).FirstOrDefault();
            }
        }

        //filtrleme
        public List<Product> GetProductsByCategory(string name)
        {
           using(var context=new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if(!string.IsNullOrEmpty(name))
                {
                    products = products.Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == name.ToLower()));
                }
                return products.ToList();
            }
        }

        public List<Product> GetTop5Products()
        {
            throw new NotImplementedException();
        }
    }
}
