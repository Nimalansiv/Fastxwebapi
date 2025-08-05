using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastxWebApi.Repository
{
    public abstract class RepositoryDB<K, T> : IRepository<K, T> where T : class
    {
        protected readonly ApplicationDbContext _context;


        public RepositoryDB(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Delete(K key)
        {
            var obj = await GetById(key);
            if (obj != null)
            {
                _context.Set<T>().Remove(obj);
                await _context.SaveChangesAsync();
            }
            return obj;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public abstract Task<T> GetById(K key);

        public virtual async Task<T> Update(K key, T entity)
        {
            var obj = await GetById(key);
            if (obj != null)
            {
                _context.Entry(obj).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
            return entity;
        }
    }
}