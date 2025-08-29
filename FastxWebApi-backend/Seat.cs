<<<<<<< HEAD:FastxWebApi-backend/Seat.cs
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FastxWebApi.Interfaces;

namespace FastxWebApi.Models
{
    public class Seat
    {
        
        public int SeatId {  get; set; }
        public int RouteId {  get; set; }
        

        public Route Route { get; set; }

        public string SeatNumber {  get; set; } = string.Empty;

        public ICollection<BookingSeat>BookingSeats { get; set; }
        public bool IsBooked { get; set; }

        public string Status { get; set; } = "Active";
        public bool IsDeleted { get; set; } = false;
    }
}
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FastxWebApi.Interfaces;

namespace FastxWebApi.Models
{
    public class Seat
    {
        
        public int SeatId {  get; set; }
        public int RouteId {  get; set; }
        

        public Route Route { get; set; }

        public string SeatNumber {  get; set; } = string.Empty;

        public ICollection<BookingSeat>BookingSeats { get; set; }
        public bool IsBooked { get; set; }

        public string Status { get; set; } = "Active";
        public bool IsDeleted { get; set; } = false;

        public int BusId { get; set; }
        public Bus Bus { get; set; }
    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/Seat.cs
