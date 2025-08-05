using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    public class User
    {
       
        public int UserId {  get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public int RoleId {  get; set; }
       

        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? BusId { get; set; }
        

        public Bus?Bus { get; set; }

        public ICollection<Refund> ProcessedRefunds { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
