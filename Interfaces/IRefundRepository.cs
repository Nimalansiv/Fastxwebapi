using FastxWebApi.Models;

namespace FastxWebApi.Interfaces
{
    public interface IRefundRepository:IRepository<int,Refund>
    {
        Task<IEnumerable<Refund>> GetPendingRefunds();
        Task<Refund> GetRefundByBookingId(int bookingId);
        Task<IEnumerable<Refund>> GetRefundsByUserId(int userId);





    }
}
