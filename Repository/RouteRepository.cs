using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Route = FastxWebApi.Models.Route;

namespace FastxWebApi.Repository
{
    public class RouteRepository : RepositoryDB<int, Route>, IRouteRepository
    {
       // private readonly ApplicationDbContext _context;

        public RouteRepository(ApplicationDbContext context) : base(context)
        {
           // _context = context;
        }

       
        public async Task<IEnumerable<Route>> SearchRoutes(string origin, string destination, DateTime travelDate)
        {
            return await _context.Routes.Where(r => r.Origin == origin && r.Destination == destination && r.DepartureTime.Date == travelDate.Date).ToListAsync();
        }

        
        public async Task<IEnumerable<Route>> GetRoutesByBusId(int busId)
        {
            return await _context.Routes.Where(r => r.BusId == busId).ToArrayAsync();
        }

        

        public override async Task<Route> GetById(int key)
        {
            return await _context.Routes.SingleOrDefaultAsync(r => r.RouteId == key);
        }

        public override async Task<IEnumerable<Route>> GetAll()
        {
            return await _context.Routes.ToListAsync();
        }

        public override async Task<Route> Add(Route entity)
        {
            _context.Routes.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<Route> Update(int key, Route entity)
        {
            var existingRoute = await _context.Routes.FindAsync(key);
            if (existingRoute == null)
            {
                return null;
            }
            _context.Entry(existingRoute).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return existingRoute;
        }

        public override async Task<Route> Delete(int key)
        {
            var route = await _context.Routes.FindAsync(key);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
            return route;
        }
    }
}