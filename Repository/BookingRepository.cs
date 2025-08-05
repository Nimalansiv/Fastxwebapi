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
            return await _context.Bookings.SingleOrDefaultAsync(b => b.BookingId == key);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByBusId(int busId)
        {
            return await _context.Bookings.Where(b => b.Route.BusId == busId).ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserId(int userId)
        {
            return await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
        }


        public async Task<IEnumerable<object>> GetSeatsByBookingId(int bookingId, int userId)
        {
            var seats = await _context.BookingsSeats.Include(bs => bs.Seat).Where(bs => bs.BookingId == bookingId && bs.Booking.UserId == userId)
                .Select(bs => new SeatDTO{ SeatId = bs.Seat.SeatId, SeatNumber =bs.Seat.SeatNumber }).ToListAsync();

            return seats;
        }


    }
}
