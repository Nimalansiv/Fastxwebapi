using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    public class Refund
    {
        
        public int RefundId {  get; set; }

        public int BookingId {  get; set; }
        

        public Booking Booking { get; set; }

        public decimal RefundAmount {  get; set; }
        public DateTime RefundDate { get; set; } = DateTime.Now;

        public int? ProcessedBy { get; set; }
        

        public User? ProcessedByUser { get; set; }

        public int UserId {  get; set; }
        public User? User { get; set; }

        public string Status { get; set; } = "Pending";










    }
}
