using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    public class Seat
    {
        
        public int SeatId {  get; set; }
        public int RouteId {  get; set; }
        

        public Route Route { get; set; }

        public string SeatNumber {  get; set; } = string.Empty;

        public ICollection<BookingSeat>BookingSeats { get; set; }






    }
}
