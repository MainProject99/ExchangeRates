﻿using DLL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Repositiries
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly ApplicationDbContext Database;
        protected readonly DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            Database = context;
            entities = context.Set<T>();
        }


        public T Insert(T entity)
        {
            if (entity == null)
            {
                throw new NotImplementedException();
            }
            entities.Add(entity);
            return entity;
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new NotImplementedException();
            entities.Update(entity);
            return entity;
        }


        public IQueryable<T> Get(string includeProperties = "")
        {
            IQueryable<T> query = entities;
            foreach (var includeProperty in
                includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query;
        }


        public T Get(int id)
        {
            return entities.Find(id);
        }

        public T Delete(T entity)
        {
            if (entity == null)
            {
                throw new NotImplementedException();
            }
            entities.Remove(entity);
            return entity;
        }
    }
}
