using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class UserRepository : RepositoryDB<int, User>, IUserRepository
    {
        //private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<User> GetById(int key)
        {
            return await _context.Users.Include(u=>u.Role).SingleOrDefaultAsync(u => u.UserId == key);
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Name == username);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Email == email);
        }






    }
}
