using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TestCurrency.Models;

namespace TestCurrency.Data.Interfaces
{
   public interface IAsyncRepository<T> where T : BaseEntity
   {
       Task<T> GetById(int id);
       Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
       void Add(T entity);
       void Update(T entity);
       void Remove(T entity);
       Task<IEnumerable<T>> GetAll();
       Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate);
        Task<bool> SaveAll();
        bool EntityExists(int id);
   }
}
