using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    public class Payment
    {
        
        public int PaymentId {  get; set; }

        public int BookingId {  get; set; }

        
        public Booking?  Booking { get; set; } 

        public decimal Amount {  get; set; }

        public string PaymentMethod { get; set; } = "Upi";

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Completed";

        public int UserId { get; set; }

        public User? User { get; set; }








    }
}
