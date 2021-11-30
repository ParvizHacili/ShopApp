﻿using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopApp.Data.Concrete.EfCore
{
    public class EfCoreGenericRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
       
    {
        protected readonly DbContext context;
        public EfCoreGenericRepository(DbContext ctx)
        {
            context = ctx;
        }
        public void Create(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }

        public List<TEntity> GetAll()
        {
            return context.Set<TEntity>().ToList();
        }

        public TEntity GetByID(int id)
        {

            return context.Set<TEntity>().Find(id);
        }

        public virtual void Update(TEntity entity)
        {
          
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
