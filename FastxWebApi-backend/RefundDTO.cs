namespace FastxWebApi.Models.DTOs
{
    public class RefundDTO
    {
        public int RefundId {  get; set; }

        public int BookingId {  get; set; }

        public  decimal RefundAmount {  get; set; }

        public DateTime RefundDate { get; set; }

        public int? ProcessedBy { get; set; }
        public int UserId {  get; set; }

        public string Status { get; set; } = "Pending";







    }
}
