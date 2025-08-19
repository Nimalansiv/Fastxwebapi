using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    public class Route
    {
        
        public int RouteId {  get; set; }
        public int BusId {  get; set; }
        

        public Bus Bus { get; set; }

        public string Origin { get; set; } = string.Empty;

        public string Destination { get; set; } = string.Empty;

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public Decimal Fare {  get; set; }

        public ICollection<Seat>? Seats { get; set; }
        public string Status { get; set; } = "Active";

        public bool IsDeleted { get; set; } = false;




    }
}
