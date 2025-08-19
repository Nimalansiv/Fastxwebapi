using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IRefundService
    {
        public Task<string> ApproveRefund(int refundId, int ProcessedByUserId);

        public Task<PagenationResponseDTO<RefundDTO>> GetAllRefunds(PagenationRequestDTO pagenation);

        public Task<PagenationResponseDTO<RefundDTO>> GetPendingRefunds(PagenationRequestDTO pagenation);

        public Task<RefundDTO> GetRefundByBookingId(int bookingId);

        public Task<PagenationResponseDTO<RefundDTO>> GetRefundsByUserId(int userId,PagenationRequestDTO pagenation);

    }
}
