using ShopApp.Business.Abstract;
using ShopApp.Data.Abstract;
using ShopApp.Data.Concrete.EfCore;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;
        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public bool Create(Product entity)
        {
            if(Validation(entity))
            {
                _productRepository.Create(entity);
                return true;
            }
            return false;
        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetByID(int id)
        {
            return _productRepository.GetByID(id);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public Product GetProductDetails(string url)
        {
           return _productRepository.GetProductDetails(url);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name,page,pageSize);
        }

        public List<Product> GetSearchResult(string searchString)
        {
            return _productRepository.GetSearchResult(searchString);
        }

        public void Update(Product entity)
        {
             _productRepository.Update(entity);
        }

        public void Update(Product entity, int[] categoryIds)
        {
            _productRepository.Update(entity, categoryIds);
        }

        public string ErrorMessage { get ; set; }

        public bool Validation(Product entity)
        {
            var IsValid = true;

            if(string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "Məhsul adı boş ola bilməz!\n";
                IsValid = false;
            }

            if (entity.Price<0 || entity.Price==0)
            {
                ErrorMessage += "Məhsul qiyməti mənfi və 0 ola bilməz!\n";
                IsValid = false;
            }

            return IsValid;
        }
    }
}
