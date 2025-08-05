namespace FastxWebApi.Models.DTOs
{
    public class UserDTO
    {
        public int UserId {  get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Gender {  get; set; } = string.Empty;

        public string ContactNumber {  get; set; } = string.Empty;
        public string Address {  get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public int? BusId { get; set; }











    }
}
