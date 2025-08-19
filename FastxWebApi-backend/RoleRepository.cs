using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class RoleRepository : RepositoryDB<int, Role>, IRepository<int, Role>
    {
        public RoleRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Role> GetById(int key)
        {
            return await _context.Roles.SingleOrDefaultAsync(r => r.RoleId == key);
        }

        
        public override async Task<PagenationResponseDTO<Role>> GetAll(PagenationRequestDTO pagination)
        {
            var query = _context.Roles.AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagenationResponseDTO<Role>
            {
                TotalCount = totalCount,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Items = items
            };
        }

       
        public override Task<bool> SoftDelete(int key)
        {
            throw new NotSupportedException("Role entities cannot be soft-deleted.");
        }
    }
}
