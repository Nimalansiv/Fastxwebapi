using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IRefundRepository:IRepository<int,Refund>
    {
        Task<PagenationResponseDTO<Refund>> GetPendingRefunds(PagenationRequestDTO pagenation);
        Task<Refund> GetRefundByBookingId(int bookingId);
        Task<PagenationResponseDTO<Refund>> GetRefundsByUserId(int userId,PagenationRequestDTO pagenation);

    }
}
