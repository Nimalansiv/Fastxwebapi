using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class RouteDTO
    {
        public int RouteId {  get; set; }

        [Required(ErrorMessage = "Bus Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Bus Id must be a positive number.")]
        public int BusId {  get; set; }


        public string BusName {  get; set; } = string.Empty;

        public string BusType {  get; set; } = string.Empty;

        public string Amenities {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Origin is required.")]
        public string Origin {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Destination is required.")]
        public string Destination { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departure time is required.")]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival time is required.")]
        public DateTime ArrivalTime { get; set; }

        [Required(ErrorMessage = "Fare is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Fare must be a positive value.")]
        public decimal Fare {  get; set; }








    }
}
