<<<<<<< HEAD:FastxWebApi-backend/BookingSeat.cs
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FastxWebApi.Interfaces;

namespace FastxWebApi.Models
{
    public class BookingSeat
    {
        
        public int Id { get; set; }

        public int BookingId {  get; set; }

        
        public Booking? Booking { get; set; }

        
        public int SeatId {  get; set; }
        

        public Seat? Seat { get; set; }


       



    }
}
=======
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FastxWebApi.Interfaces;

namespace FastxWebApi.Models
{
    public class BookingSeat
    {
        
        public int Id { get; set; }

        public int BookingId {  get; set; }

        
        public Booking? Booking { get; set; }

        
        public int SeatId {  get; set; }
        

        public Seat? Seat { get; set; }


       



    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/BookingSeat.cs
