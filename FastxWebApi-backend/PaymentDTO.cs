using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class PaymentDTO
    {
        [Required(ErrorMessage = "Booking Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Booking Id must be a positive number.")]
        public int BookingId {  get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Amount must be a positive value.")]
        public decimal Amount {  get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; } = string.Empty;

        [Required(ErrorMessage = "User Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "User Id must be a positive number.")]
        public int UserId {  get; set; }

        public string PaymentStatus {  get; set; }






    }
}
