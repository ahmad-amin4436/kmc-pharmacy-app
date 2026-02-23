using System;
using System.Collections.Generic;

namespace Ph_App.DAL
{
    public interface IRepository<T> where T : class
    {
        // Basic CRUD operations
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetByFilter(string filter);
        T Add(T entity);
        T Update(T entity);
        bool Delete(int id);
        bool Delete(T entity);
        int Count();
        int CountByFilter(string filter);
    }
}
