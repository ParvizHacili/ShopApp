using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Data.Abstract
{
   public interface IProductRepository:IRepository<Product>
    {
        Product GetProductDetails(int id);
       List<Product> GetProductsByCategory(string name);
        List<Product> GetPopularProducts();
        List<Product> GetTop5Products();
    }
}
