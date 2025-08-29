<<<<<<< HEAD:FastxWebApi-backend/PagenationResponseDTO.cs
﻿namespace FastxWebApi.Models.DTOs
{
    public class PagenationResponseDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
    }
}
=======
﻿namespace FastxWebApi.Models.DTOs
{
    public class PagenationResponseDTO<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Models/DTOs/PagenationResponseDTO.cs
