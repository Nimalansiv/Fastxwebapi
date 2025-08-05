using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserDTO>> GetAllUsers();

        public Task<UserDTO> GetUserById(int id);
        public Task<string> DeleteUser(int id);

        public Task<string> UpdateUserProfile(UpdateUserDTO dto);

        public Task<string> RegisterUser(RegisterDTO dto);
        public Task<string> ChangeUserRole(int userId, int newRoleId);



    }
}
