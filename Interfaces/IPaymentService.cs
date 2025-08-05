



using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IPaymentService
    {
        public Task<string> ProcessPayment(PaymentDTO paymentDTO);

        public Task<IEnumerable<PaymentDisplayDTO>> GetAllPayments();

        public Task<PaymentDisplayDTO> GetPaymentByBookingId(int bookingId);

        public Task<IEnumerable<PaymentDisplayDTO>> GetPaymentsByUserId(int userId);











    }
}
