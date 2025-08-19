using System;
using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class BookingRepository : RepositoryDB<int, Booking>,IBookingRepository
    {
        

        public BookingRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Booking> GetById(int key)
        {
            return await _context.Bookings.Include(b => b.BookingSeats).ThenInclude(bs => bs.Seat).SingleOrDefaultAsync(b => b.BookingId == key && !b.IsDeleted);
        }

       
        public async Task<PagenationResponseDTO<Booking>> GetBookingsByBusId(int busId, PagenationRequestDTO pagination)
        {
            var query = _context.Bookings.Include(b => b.Route)  .Where(b => b.Route.BusId == busId && !b.IsDeleted);

            return await GetPaginatedResult(query, pagination);
        }

        
        public async Task<PagenationResponseDTO<Booking>> GetBookingsByUserId(int userId, PagenationRequestDTO pagination)
        {
            var query = _context.Bookings.Where(b => b.UserId == userId && !b.IsDeleted);
            return await GetPaginatedResult(query, pagination);
        }

        
        public async Task<IEnumerable<Seat>> GetSeatsByBookingId(int bookingId, int userId)
        {
            return await _context.BookingSeats.Include(bs => bs.Seat).Where(bs => bs.BookingId == bookingId && bs.Booking.UserId == userId).Select(bs => bs.Seat).ToListAsync();
        }

        public async Task<bool> SoftDelete(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return false;

            booking.Status = "Deleted";
            booking.IsDeleted = true;
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return true;
        }

        public override async Task<PagenationResponseDTO<Booking>> GetAll(PagenationRequestDTO pagination)
        {
            var query = _context.Bookings.Where(b => !b.IsDeleted);
            return await GetPaginatedResult(query, pagination);
        }

        
        private async Task<PagenationResponseDTO<Booking>> GetPaginatedResult(IQueryable<Booking> query, PagenationRequestDTO pagination)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();

            return new PagenationResponseDTO<Booking>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pagination.PageSize,
                CurrentPage = pagination.PageNumber
            };
        }




    }
}
