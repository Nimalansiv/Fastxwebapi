using FastxWebApi.Context;
using FastxWebApi.Interfaces;
using FastxWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FastxWebApi.Repository
{
    public class RefundRepository: RepositoryDB<int, Refund>, IRefundRepository
    {
        
        public RefundRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Refund>>GetPendingRefunds()
        {
            return await _context.Refunds.Where(r=>r.Status == "Pending").ToListAsync();
        }


        public async Task<Refund> GetRefundByBookingId(int bookingId)
        {
            return await _context.Refunds.SingleOrDefaultAsync(r => r.BookingId == bookingId);
        }

        public async Task<IEnumerable<Refund>> GetRefundsByUserId(int userId)
        {
            return await _context.Refunds.Where(r => r.Booking.UserId== userId).ToListAsync();
        }

        public override async Task<Refund> GetById(int key)
        {
            return await _context.Refunds.SingleOrDefaultAsync(r => r.RefundId == key);
        }


    }
}
