using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
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
            
            return await _context.BookingSeats.Include(bs => bs.Booking).SingleOrDefaultAsync(bs => bs.Id == key && !bs.Booking.IsDeleted);
        }

        
        public override async Task<PagenationResponseDTO<BookingSeat>> GetAll(PagenationRequestDTO pagenation)
        {
            var query = _context.BookingSeats.Where(bs => !bs.Booking.IsDeleted);
            var totalCount = await query.CountAsync();

            var items = await query.Skip((pagenation.PageNumber - 1) * pagenation.PageSize).Take(pagenation.PageSize).ToListAsync();

            return new PagenationResponseDTO<BookingSeat>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pagenation.PageSize,
                CurrentPage = pagenation.PageNumber
            };
        }

        
        public override Task<bool> SoftDelete(int key)
        {
            throw new NotSupportedException("BookingSeat entities cannot be soft deleted directly.");
        }



    }
}
