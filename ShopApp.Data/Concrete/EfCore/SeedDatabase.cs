using Microsoft.EntityFrameworkCore;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.Data.Concrete.EfCore
{
   public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            }
            context.SaveChanges();
        }

        private static Category[] Categories =
        {
            new Category(){Name="Telefon",Url="telefon"},
            new Category(){Name="Kompyuter",Url="kompyuter"},
            new Category(){Name="Elektronika",Url="elektronika"},
            new Category(){Name="İkinci Əl",Url="ikinci-əl"}
        };

        private static Product[] Products =
       {
            new Product(){Name="Samsung s5",Url="samsung-s5",Price=20,Description="Fine",ImageUrl="2.jpg",IsApproved=true},
            new Product(){Name="Samsung s6",Url="samsung-s6",Price=30,Description="Fine",ImageUrl="3.jpg",IsApproved=false},
            new Product(){Name="Samsung s7",Url="samsung-s7",Price=40,Description="Fine",ImageUrl="4.jpg",IsApproved=true},
            new Product(){Name="Samsung s8",Url="samsung-s8",Price=50,Description="Fine",ImageUrl="5.jpg",IsApproved=false},
            new Product(){Name="Samsung s9",Url="samsung-s9",Price=60,Description="Fine",ImageUrl="1.jpg",IsApproved=true},  
        };

        private static ProductCategory[] ProductCategories =
        {
            new ProductCategory(){Product=Products[0],Category=Categories[0]},
            new ProductCategory(){Product=Products[0],Category=Categories[2]},
            new ProductCategory(){Product=Products[1],Category=Categories[0]},
            new ProductCategory(){Product=Products[1],Category=Categories[2]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[2],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]},
            new ProductCategory(){Product=Products[3],Category=Categories[2]}
        };
    }
}
