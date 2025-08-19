namespace FastxWebApi.Models.DTOs
{
    public class BookingResponseDTO
    {
        public int BookingId {  get; set; }

        public int UserId {  get; set; }

        public int RouteId {  get; set; }

        public DateTime BookingDate { get; set; }

        public int NoOfSeats { get; set; }

        public decimal TotalFare {  get; set; }
        public string Status { get; set; } = "Booked";

        public string Origin {  get; set; }

        public string Destination {  get; set; }

        public DateTime DepartureTime { get; set; }









    }
}
