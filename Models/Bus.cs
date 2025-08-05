using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models
{
    public class Bus
    {
        
        public int BusId {  get; set; }

        public string BusName { get; set; } = string.Empty;

        public string BusNumber { get; set; } = string.Empty;
        public string BusType {  get; set; } = string.Empty;
        public int TotalSeats {  get; set; }

        public string Amenities { get; set; } = string.Empty;
        public ICollection<Route>? Routes { get; set; }

        public User? BusOperator { get; set; }
    }
}
