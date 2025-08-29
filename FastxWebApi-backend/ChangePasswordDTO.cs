<<<<<<< HEAD:FastxWebApi-backend/ChangePasswordDTO.cs
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "User Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User Id must be a positive number.")]
        public int UserId {  get; set; }

        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;


    }
}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "User Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User Id must be a positive number.")]
        public int UserId {  get; set; }

        [Required(ErrorMessage = "Current password is required.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;


    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/DTOs/ChangePasswordDTO.cs
