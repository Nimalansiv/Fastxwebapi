using FastxWebApi.Context;
using FastxWebApi.Models;
using FastxWebApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace FastxWebApi.Repository
{
    public class PaymentRepository: RepositoryDB<int, Payment>, IPaymentRepository
    {
        
        public PaymentRepository(ApplicationDbContext context) : base(context) { }


        public async Task<Payment> GetPaymentByBookingId(int bookingId)
        {
            return await _context.Payments.SingleOrDefaultAsync(p=>p.BookingId == bookingId);
        }
        public async Task<IEnumerable<Payment>> GetPaymentsByUserId(int userId)
        {
            return await _context.Payments.Where(p => p.Booking.UserId == userId).ToListAsync();
        }

        public override async Task<Payment> GetById(int key)
        {
            return await _context.Payments.SingleOrDefaultAsync(p => p.PaymentId == key);
        }






    }
}
