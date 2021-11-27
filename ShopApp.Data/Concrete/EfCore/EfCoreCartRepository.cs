using ShopApp.Data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Data.Concrete.EfCore
{
    public class EfCoreCartRepository: EfCoreGenericRepository<Cart, ShopContext>, ICartRepository
    {

    }
}
