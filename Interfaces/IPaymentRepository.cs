using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface IPaymentRepository:IRepository<int,Payment>
    {
        Task<Payment> GetPaymentByBookingId(int bookingId);

       
        Task<IEnumerable<Payment>> GetPaymentsByUserId(int userId);





    }
}
