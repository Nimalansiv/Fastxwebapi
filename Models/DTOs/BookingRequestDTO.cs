using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class BookingRequestDTO
    {
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId {  get; set; }

        [Required(ErrorMessage = "Route Id is required.")]
        public int RouteId {  get; set; }

        [Required(ErrorMessage = "Seat numbers are required.")]
        [MinLength(1, ErrorMessage = "At least one seat must be selected.")]
        public List<string> SeatNumbers { get; set; } = new List<string>();

    }
}
