using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class RoleRepository : RepositoryDB<int, Role>, IRepository<int, Role>
    {
        public RoleRepository(ApplicationDbContext context) : base(context) { }

        public async override Task<Role> GetById(int key)
        {
            return await _context.Roles.SingleOrDefaultAsync(r => r.RoleId == key);
        }
    }
}
