using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class CancelSeatsDTO
    {
        [Required(ErrorMessage = "Booking Id is required.")]
        public int BookingId {  get; set; }

        [Required(ErrorMessage = "Seat Ids are required.")]
        [MinLength(1, ErrorMessage = "At least one seat must be selected.")]
        public List<int> SeatIds { get; set; } = new List<int>();





    }
}
