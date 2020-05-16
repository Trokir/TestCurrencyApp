using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestCurrency.Data.Interfaces;
using TestCurrency.Models;

namespace TestCurrency.Data.Repos
{
    public  class BaseRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;

        #region Ctors
   protected BaseRepository(DataContext context)
        {
            _context = context;
        }

        #endregion

        #region Base Methods
         /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate) 
            => _context.Set<T>().FirstOrDefaultAsync(predicate);
        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<T> GetById(int id)
            => await _context.Set<T>().FindAsync(id);
        /// <summary>
        /// Gets the where.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        /// <summary>
        /// Adds the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Add(T entity)
        {
             _context.Add(entity);
        }
        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Remove(T entity)
        {
            _context.Remove(entity);
        }
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
        /// <summary>
        /// Saves all.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

      
        public bool EntityExists(int id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        #endregion


    }
}
