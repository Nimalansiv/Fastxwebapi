using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IRefundService
    {

        public Task<string> ApproveRefund(int refundId, int ProcessedByUserId);

        public Task<IEnumerable<RefundDTO>> GetAllRefunds();

        public Task<IEnumerable<RefundDTO>> GetPendingRefunds();

        public Task<RefundDTO> GetRefundByBookingId(int bookingId);

        public Task<IEnumerable<RefundDTO>> GetRefundsByUserId(int userId);





    }
}
