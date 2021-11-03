using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Data.Abstract
{
   public interface IProductRepository:IRepository<Product>
    {
        List<Product> GetPopularProducts();
    }
}
