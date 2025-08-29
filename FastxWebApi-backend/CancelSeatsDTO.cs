<<<<<<< HEAD:FastxWebApi-backend/CancelSeatsDTO.cs
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class CancelSeatsDTO
    {
        [Required(ErrorMessage = "Booking Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Booking Id must be a positive number.")]
        public int BookingId {  get; set; }

        [Required(ErrorMessage = "Seat Ids are required.")]
        [MinLength(1, ErrorMessage = "At least one seat must be selected.")]
        public List<int> SeatIds { get; set; } = new List<int>();





    }
}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class CancelSeatsDTO
    {
        [Required(ErrorMessage = "Booking Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Booking Id must be a positive number.")]
        public int BookingId {  get; set; }

        [Required(ErrorMessage = "Seat Ids are required.")]
        [MinLength(1, ErrorMessage = "At least one seat must be selected.")]
        public List<int> SeatIds { get; set; } = new List<int>();





    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/DTOs/CancelSeatsDTO.cs
