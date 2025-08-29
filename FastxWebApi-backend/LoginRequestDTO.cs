<<<<<<< HEAD:FastxWebApi-backend/LoginRequestDTO.cs
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }
}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/DTOs/LoginRequestDTO.cs
