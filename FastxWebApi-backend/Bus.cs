<<<<<<< HEAD:FastxWebApi-backend/Bus.cs
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    [Table("Buses")]
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
        public int? BusOperatorId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public string Status { get; set; } = "Active";

    }
}
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastxWebApi.Models
{
    [Table("Buses")]
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
        public int? BusOperatorId { get; set; }

        public bool IsDeleted { get; set; } = false;
        public string Status { get; set; } = "Active";

        public ICollection<Seat> Seats { get; set; }

    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/Bus.cs
