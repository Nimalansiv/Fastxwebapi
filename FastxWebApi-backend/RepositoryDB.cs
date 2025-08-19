using FastxWebApi.Context;
using FastxWebApi.Exceptions;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        

        public abstract Task<T> GetById(K key);

       
        public virtual async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

      
        public abstract Task<PagenationResponseDTO<T>> GetAll(PagenationRequestDTO pagination);

      
        public virtual async Task<bool> SoftDelete(K key)
        {
            var entity = await GetById(key);
            if (entity == null) return false;

            var propIsDeleted = entity.GetType().GetProperty("IsDeleted");
            var propStatus = entity.GetType().GetProperty("Status");

            if (propIsDeleted != null)
            {
                propIsDeleted.SetValue(entity, true);
            }
            if (propStatus != null)
            {
                propStatus.SetValue(entity, "Deleted");
            }

            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}