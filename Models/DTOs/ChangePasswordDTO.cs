using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "User Id is required.")]
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
