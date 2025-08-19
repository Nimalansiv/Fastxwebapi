using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IPaymentRepository:IRepository<int,Payment>
    {
        Task<Payment> GetPaymentByBookingId(int bookingId);
        Task<PagenationResponseDTO<Payment>> GetPaymentsByUserId(int userId,PagenationRequestDTO pagenation);

    }
}
