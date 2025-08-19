using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class RefundRepository: RepositoryDB<int, Refund>, IRefundRepository
    {
        
        public RefundRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<PagenationResponseDTO<Refund>> GetAll(PagenationRequestDTO pagination)
        {
            var query = _context.Refunds.AsNoTracking();
            return await GetPaginatedResult(query, pagination);
        }

        public async Task<PagenationResponseDTO<Refund>> GetPendingRefunds(PagenationRequestDTO pagination)
        {
            var query = _context.Refunds.Where(r => r.Status == "Pending").AsNoTracking();
            return await GetPaginatedResult(query, pagination);
        }

        public async Task<Refund> GetRefundByBookingId(int bookingId)
        {
            return await _context.Refunds.SingleOrDefaultAsync(r => r.BookingId == bookingId);
        }

        public async Task<PagenationResponseDTO<Refund>> GetRefundsByUserId(int userId, PagenationRequestDTO pagination)
        {
            var query = _context.Refunds.Where(r => r.UserId == userId).AsNoTracking();
            return await GetPaginatedResult(query, pagination);
        }

        public override async Task<Refund> GetById(int key)
        {
            return await _context.Refunds.SingleOrDefaultAsync(r => r.RefundId == key);
        }

       
        private async Task<PagenationResponseDTO<Refund>> GetPaginatedResult(IQueryable<Refund> query, PagenationRequestDTO pagination)
        {
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagenationResponseDTO<Refund>
            {
                TotalCount = totalCount,
                CurrentPage = pagination.PageNumber,
                PageSize = pagination.PageSize,
                Items = items
            };
        }

    }
}
