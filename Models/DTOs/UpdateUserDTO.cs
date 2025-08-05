using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "User Id is required.")]
        public int UserId {  get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender {  get; set; } = string.Empty;


        [Required(ErrorMessage = "Contact number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be a 10-digit number.")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required.")]
        public string Address {  get; set; } = string.Empty;

        public int? BusId {  get; set; }

        





    }
}
