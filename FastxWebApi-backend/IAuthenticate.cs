using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IAuthenticate
    {
        public Task<RegisterUserResponseDTO> RegisterUser(RegisterUserRequestDTO employee);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
    }
}
