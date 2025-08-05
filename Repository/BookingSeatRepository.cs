using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class BookingSeatRepository : RepositoryDB<int, BookingSeat>, IRepository<int, BookingSeat>
    {
       

        public BookingSeatRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public override async Task<BookingSeat> GetById(int key)
        {
            return await _context.BookingsSeats.SingleOrDefaultAsync(bs => bs.Id == key);
        }



    }
}
