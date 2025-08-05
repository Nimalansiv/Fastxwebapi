using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class SeatRepository : RepositoryDB<int, Seat>, ISeatRepository
    {
        //private readonly ApplicationDbContext _context;

        public SeatRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Seat>>GetSeatsByRoute(int routeId)
        {

            return await _context.Seats.Where(s=>s.RouteId == routeId).ToListAsync();
        }
        public override async Task<Seat> GetById(int key)
        {
            return await _context.Seats.SingleOrDefaultAsync(s => s.SeatId == key);
        }

    }
}
