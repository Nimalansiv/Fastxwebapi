using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IPaymentService
    {
        public Task<string> ProcessPayment(PaymentDTO paymentDTO);

        public Task<PagenationResponseDTO<PaymentDisplayDTO>> GetAllPayments(PagenationRequestDTO pagenation);

        public Task<PaymentDisplayDTO> GetPaymentByBookingId(int bookingId);

        public Task<PagenationResponseDTO<PaymentDisplayDTO>> GetPaymentsByUserId(int userId, PagenationRequestDTO pagenation);

    }
}
