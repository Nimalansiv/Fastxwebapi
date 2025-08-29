<<<<<<< HEAD:FastxWebApi-backend/IRepository.cs
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        
        Task<T> GetById(K key);
        Task<PagenationResponseDTO<T>> GetAll(PagenationRequestDTO pagination);
        Task<bool> SoftDelete(K key);

    }
=======
﻿using System.Collections.Generic;
using System.Threading.Tasks;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        
        Task<T> GetById(K key);
        Task<PagenationResponseDTO<T>> GetAll(PagenationRequestDTO pagination);
        Task<bool> SoftDelete(K key);

    }
>>>>>>> e40ecec (initial commit - backend fastx):Interfaces/IRepository.cs
}