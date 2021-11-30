using ShopApp.Business.Abstract;
using ShopApp.Data.Abstract;
using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(Category entity)
        {
            _unitOfWork.Categories.Create(entity);
            _unitOfWork.Save();
        }

        public void Delete(Category entity)
        {
            _unitOfWork.Categories.Delete(entity);
            _unitOfWork.Save();
        }

        public void DeleteFromCategory(int productId, int categoryId)
        {
            _unitOfWork.Categories.DeleteFromCategory(productId, categoryId);
        }

        public List<Category> GetAll()
        {
            return _unitOfWork.Categories.GetAll();
        }

        public Category GetById(int id)
        {
            return _unitOfWork.Categories.GetByID(id);
        }

        public Category GetByIdWithProducts(int categoryId)
        {
            return _unitOfWork.Categories.GetByIdWithProducts(categoryId);
        }

        public void Update(Category entity)
        {
            _unitOfWork.Categories.Update(entity);
            _unitOfWork.Save();
        }
        public string ErrorMessage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Validation(Category entity)
        {
            throw new NotImplementedException();
        }
    }
}
