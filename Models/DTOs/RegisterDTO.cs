using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty ;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        [StringLength(20, ErrorMessage = "Gender cannot be longer than 20 characters.")]
        public string Gender {  get; set; } = string.Empty ;

        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be a 10-digit number.")]
        public string ContactNumber {  get; set; } = string.Empty ;

        [Required(ErrorMessage = "Address is required.")]
        public string Address {  get; set; } = string.Empty ;

        [Required(ErrorMessage = "Role is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Role Id must be a positive number.")]
        public int RoleId {  get; set; }







    }
}
