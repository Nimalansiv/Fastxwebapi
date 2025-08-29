using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class BusRepository : RepositoryDB<int,Bus>,IBusRepository
    {
       
        public BusRepository(ApplicationDbContext context) : base(context) 
        {
          
        
        }

        public override async Task<Bus> GetById(int key)
        {
            return await _context.Buses.SingleOrDefaultAsync(b => b.BusId == key && !b.IsDeleted);
        }

        public override async Task<PagenationResponseDTO<Bus>> GetAll(PagenationRequestDTO pagination)
        {
            var query = _context.Buses.Where(b => !b.IsDeleted).AsNoTracking();

            return await GetPaginatedResult(query, pagination);
        }

        public async Task<PagenationResponseDTO<Bus>> FindAvailableBuses(int routeId, DateTime travelDate, PagenationRequestDTO pagination)
        {
            var query = _context.Buses.Where(b => !b.IsDeleted && b.Routes.Any(r => r.RouteId == routeId && r.DepartureTime.Date == travelDate.Date)).AsNoTracking();

            return await GetPaginatedResult(query, pagination);
        }

        public async Task<bool> SoftDelete(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null) return false;

            bus.IsDeleted = true;
            _context.Buses.Update(bus);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<PagenationResponseDTO<Bus>> GetPaginatedResult(IQueryable<Bus> query, PagenationRequestDTO pagination)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagenationResponseDTO<Bus>
            {
                TotalCount = totalCount,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Items = items
            };
        }

    }
}
