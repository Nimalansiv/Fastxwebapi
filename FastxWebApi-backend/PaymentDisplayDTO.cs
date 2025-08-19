namespace FastxWebApi.Models.DTOs
{
    public class PaymentDisplayDTO
    {
        public int PaymentId {  get; set; }

        public int BookingId {  get; set; }

        public Decimal Amount {  get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime PaymentDate { get; set; }




    }
}
