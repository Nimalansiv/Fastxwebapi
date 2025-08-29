<<<<<<< HEAD:FastxWebApi-backend/IAuthenticate.cs
﻿using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IAuthenticate
    {
        public Task<RegisterUserResponseDTO> RegisterUser(RegisterUserRequestDTO employee);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
    }
}
=======
﻿using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface IAuthenticate
    {
        public Task<RegisterUserResponseDTO> RegisterUser(RegisterUserRequestDTO employee);
        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);

        Task<string> ForgotPassword(ForgotPasswordDTO dto);
        Task<string> ChangePassword(ChangePasswordDTO dto);

    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Interfaces/IAuthenticate.cs
