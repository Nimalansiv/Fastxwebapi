using System.ComponentModel.DataAnnotations;

namespace FastxWebApi.Models
{
    public class Role
    {
        
        public int RoleId {  get; set; } 
        public string RoleName { get; set; } = string.Empty;

        public ICollection<User> User { get; set; }

    }
}
