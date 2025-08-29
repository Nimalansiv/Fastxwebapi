using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastxWebApi.Models.DTOs;

using Route = FastxWebApi.Models.Route;
using Microsoft.AspNetCore.Routing;

namespace FastxWebApi.Repository
{
    public class RouteRepository : RepositoryDB<int, Route>, IRouteRepository
    {
       

        public RouteRepository(ApplicationDbContext context) : base(context)
        {
           
        }

       
        public async Task<PagenationResponseDTO<Route>> GetRoutesByBusId(int busId, PagenationRequestDTO pagination)
        {
            var query = _context.Routes.Where(r => r.BusId == busId && !r.IsDeleted).AsNoTracking();
            return await GetPaginatedResult(query, pagination);
        }

        public override async Task<Route> GetById(int key)
        {
            return await _context.Routes.Include(r => r.Bus).SingleOrDefaultAsync(r => r.RouteId == key && !r.IsDeleted);

        }

        public override async Task<Route> Add(Route entity)
        {
            _context.Routes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

       
        public override async Task<Route> Update(Route entity)
        {
            _context.Routes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

      
        public async Task<PagenationResponseDTO<Route>> SearchRoutes(string origin, string destination, DateTime travelDate, PagenationRequestDTO pagenation)
        {
            var query = _context.Routes.Include(r=>r.Bus).Where(r => r.Origin == origin && r.Destination == destination && r.DepartureTime.Date == travelDate.Date && !r.IsDeleted);
            return await GetPaginatedResult(query, pagenation);
        }

        public async Task<bool> SoftDelete(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null) return false;

            route.Status = "Deleted";
            route.IsDeleted = true;
            _context.Routes.Update(route);
            await _context.SaveChangesAsync();
            return true;
        }

        public override async Task<PagenationResponseDTO<Route>> GetAll(PagenationRequestDTO pagination)
        {
            var query = _context.Routes.Where(r => !r.IsDeleted).AsNoTracking();
            return await GetPaginatedResult(query, pagination);
        }

       
        private async Task<PagenationResponseDTO<Route>> GetPaginatedResult(IQueryable<Route> query, PagenationRequestDTO pagination)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToListAsync();

            return new PagenationResponseDTO<Route>
            {
                Items = items,
                TotalCount = totalCount,
                PageSize = pagination.PageSize,
                CurrentPage = pagination.PageNumber
            };
        }
    }

}