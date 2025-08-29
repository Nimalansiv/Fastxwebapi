<<<<<<< HEAD:FastxWebApi-backend/PagenationRequestDTO.cs
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class PagenationRequestDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive number.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 1000, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }
}
=======
﻿using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class PagenationRequestDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be a positive number.")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 1000, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 10;
    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/DTOs/PagenationRequestDTO.cs
