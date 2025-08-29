using FastxWebApi.Context;
using FastxWebApi.Models;
using FastxWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FastxWebApi.Models.DTOs;


namespace FastxWebApi.Repository
{
    public class PaymentRepository: RepositoryDB<int, Payment>, IPaymentRepository
    {
        
        public PaymentRepository(ApplicationDbContext context) : base(context) { }

        
        public override async Task<PagenationResponseDTO<Payment>> GetAll(PagenationRequestDTO pagination)
        {
           
            var query = _context.Payments.AsNoTracking();
            return await GetPaginatedResult(query, pagination);
        }

        public async Task<Payment> GetPaymentByBookingId(int bookingId)
        {
            return await _context.Payments.SingleOrDefaultAsync(p => p.BookingId == bookingId);
        }

        public async Task<PagenationResponseDTO<Payment>> GetPaymentsByUserId(int userId, PagenationRequestDTO pagination)
        {
            var query = _context.Payments.Where(p => p.UserId == userId).AsNoTracking();
            return await GetPaginatedResult(query, pagination);
        }

        public override async Task<Payment> GetById(int key)
        {
            return await _context.Payments.SingleOrDefaultAsync(p => p.PaymentId == key);
        }

    
        private async Task<PagenationResponseDTO<Payment>> GetPaginatedResult(IQueryable<Payment> query, PagenationRequestDTO pagination)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagenationResponseDTO<Payment>
            {
                TotalCount = totalCount,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Items = items
            };
        }





    }
}
