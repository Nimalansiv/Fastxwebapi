using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class SeatRepository : RepositoryDB<int, Seat>, ISeatRepository
    {
       

        public SeatRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PagenationResponseDTO<Seat>> GetSeatsByRoute(int routeId, PagenationRequestDTO pagenation)
        {
           
            var query = _context.Seats.Where(s => s.RouteId == routeId && !s.IsDeleted);

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pagenation.PageNumber - 1) * pagenation.PageSize).Take(pagenation.PageSize).ToListAsync();

            return new PagenationResponseDTO<Seat>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pagenation.PageSize,
                CurrentPage = pagenation.PageNumber
            };
        }

        public override async Task<Seat> GetById(int key)
        {
            return await _context.Seats.SingleOrDefaultAsync(s => s.SeatId == key && !s.IsDeleted);
        }

        public async Task<bool> SoftDelete(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat == null) return false;

            seat.IsDeleted = true;
            seat.Status = "Deleted";
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
            return true;
        }

        public override async Task<PagenationResponseDTO<Seat>> GetAll(PagenationRequestDTO pagination)
        {
            var query = _context.Seats.Where(s => !s.IsDeleted);

            var totalCount = await query.CountAsync();
            var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();

            return new PagenationResponseDTO<Seat>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pagination.PageSize,
                CurrentPage = pagination.PageNumber
            };
        }
        public async Task<IEnumerable<Seat>> UpdateAll(IEnumerable<Seat> seats)
        {
            _context.Seats.UpdateRange(seats);
            await _context.SaveChangesAsync();
            return seats;
        }

        public async Task<IEnumerable<Seat>> AddRange(IEnumerable<Seat> seats)
        {
            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
            return seats;
        }

        public async Task<IEnumerable<Seat>> GetAvailableSeatsByRoute(int routeId)
        {
            return await _context.Seats
                .Where(s => s.RouteId == routeId && !s.IsBooked)
                .ToListAsync();
        }

        public async Task UpdateRange(IEnumerable<Seat> seats)
        {
            _context.Seats.UpdateRange(seats);
            await _context.SaveChangesAsync();
        }

    }
}
