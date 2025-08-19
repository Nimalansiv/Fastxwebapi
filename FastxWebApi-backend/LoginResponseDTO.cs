using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models.DTOs
{
    public class LoginResponseDTO
    {
        
        public string Email { get; set; } = string.Empty;
        public string Name {  get; set; } = string.Empty ;
        public string Token {  get; set; } = string.Empty;

        public int? UserId {  get; set; }
        public string Role { get; set; } = string.Empty;

        public int RoleId {  get; set; }



    }
}
