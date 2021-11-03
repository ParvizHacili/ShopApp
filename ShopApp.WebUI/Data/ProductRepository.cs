using ShopApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Data
{
    public static class ProductRepository
    {
        private static List<Product> _products = null;

        static ProductRepository()
        {
            _products = new List<Product>
            {
                new Product{ProductId=1,Name="Nokia",Price=50,Description="Nothing",IsApproved=true,ImageUrl="1.jpg",CategoryId=1},
                new Product{ProductId=2,Name="Iphone 5",Price=5000,Description="Good",IsApproved=false,ImageUrl="2.jpg",CategoryId=1},
                new Product{ProductId=3,Name="Iphone 6",Price=5600,Description="Good",IsApproved=true,ImageUrl="3.jpg",CategoryId=1},
                new Product{ProductId=4,Name="Iphone 7",Price=5546,Description="Good",IsApproved=true,ImageUrl="4.jpg",CategoryId=1},
                new Product{ProductId=5,Name="Lenovo",Price=50,Description="Nothing",IsApproved=true,ImageUrl="1.jpg",CategoryId=2},
                new Product{ProductId=6,Name="Lenovo",Price=5000,Description="Good",IsApproved=false,ImageUrl="2.jpg",CategoryId=2},
                new Product{ProductId=7,Name="Lenovo 6",Price=5600,Description="Good",IsApproved=true,ImageUrl="3.jpg",CategoryId=2},
                new Product{ProductId=8,Name="Lenovo 7",Price=5546,Description="Good",IsApproved=true,ImageUrl="4.jpg",CategoryId=2}
            };
        }

        public static List<Product> Products
        {
            get
            {
                return _products;
            }
        }

        public static void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public static Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.ProductId == id);
        }

        public static void EditProduct(Product product)
        {
            foreach(var p in _products)
            {
                if(p.ProductId==product.ProductId)
                {
                    p.Name = product.Name;
                    p.Price = product.Price;
                    p.Description = product.Description;
                    p.ImageUrl = product.ImageUrl;
                    p.IsApproved = product.IsApproved;
                    p.CategoryId = product.CategoryId;
                } 
            }
        }

        public static void DeleteProduct(int id)
        {
            var product = GetProductById(id);

            if(product != null)
            {
                _products.Remove(product);
            }
        }
    }
}
