using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    public class Booking
    {
        
        public int BookingId {  get; set; }

        public int UserId {  get; set; }

        
        public User? User { get; set; }

        public int RouteId {  get; set; }

        public Route? Route { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public int NoOfSeats {  get; set; }

        public decimal TotalFare {  get; set; }

        public string Status { get; set; } = "Booked";

        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

        public Payment? Payment { get; set; }

        public Refund? Refund { get; set; }



    }
}
