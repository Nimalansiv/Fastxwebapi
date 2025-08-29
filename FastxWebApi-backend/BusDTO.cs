<<<<<<< HEAD:FastxWebApi-backend/BusDTO.cs
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class BusDTO
    {


        [Range(1, int.MaxValue, ErrorMessage = "Bus Id must be a positive number.")]
        public int? BusId {  get; set; }

        [Required(ErrorMessage = "Bus name is required.")]
        public string BusName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bus number is required.")]
        public string BusNumber {  get; set; } = string.Empty;


        public string BusType { get; set; } = string.Empty ;

        [Required(ErrorMessage = "Total seats is required.")]
        [Range(1, 100, ErrorMessage = "Total seats must be between 1 and 100.")]
        public int TotalSeats {  get; set; }

        public string Amenities { get; set; } = string.Empty;







    }
}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class BusDTO
    {


        [Range(1, int.MaxValue, ErrorMessage = "Bus Id must be a positive number.")]
        public int? BusId {  get; set; }

        [Required(ErrorMessage = "Bus name is required.")]
        public string BusName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bus number is required.")]
        public string BusNumber {  get; set; } = string.Empty;


        public string BusType { get; set; } = string.Empty ;

        [Required(ErrorMessage = "Total seats is required.")]
        [Range(1, 100, ErrorMessage = "Total seats must be between 1 and 100.")]
        public int TotalSeats {  get; set; }

        public string Amenities { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bus Operator ID is required.")]
        public int BusOperatorId { get; set; }






    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/DTOs/BusDTO.cs
