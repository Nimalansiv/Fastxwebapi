using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class UserRepository : RepositoryDB<int, User>, IUserRepository
    {
        

        public UserRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<User> GetById(int key)
        {
            return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserId == key && !u.IsDeleted);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Name == username && !u.IsDeleted);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        
        public override async Task<PagenationResponseDTO<User>> GetAll(PagenationRequestDTO pagination)
        {
            var query = _context.Users.Where(u => !u.IsDeleted).AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagenationResponseDTO<User>
            {
                TotalCount = totalCount,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Items = items
            };
        }



    }
}
