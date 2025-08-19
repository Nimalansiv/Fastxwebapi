using FastxWebApi.Models;
using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IUserService
    {
        public Task<PagenationResponseDTO<UserDTO>> GetAllUsers(PagenationRequestDTO pagenation);

        public Task<UserDTO> GetUserById(int id);
        public Task<bool> SoftDeleteUser(int id);

        public Task<bool> UpdateUserProfile(UpdateUserDTO dto);

        public Task<UserDTO> RegisterUser(RegisterUserRequestDTO dto);

    }
}
