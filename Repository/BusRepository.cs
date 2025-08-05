using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class BusRepository : RepositoryDB<int,Bus>,IBusRepository
    {
       // private ApplicationDbContext _context;
        public BusRepository(ApplicationDbContext context) : base(context) 
        {
          
        
        }

        public override async Task<Bus> GetById(int Key)
        {
            return await _context.Buses.SingleOrDefaultAsync(b=>b.BusId  == Key);
        }

        public async Task<IEnumerable<Bus>> GetAvailableBuses()
        {
            
            return await _context.Buses.Where(b => b.Routes.Any()).ToListAsync();
        }
    }
}
