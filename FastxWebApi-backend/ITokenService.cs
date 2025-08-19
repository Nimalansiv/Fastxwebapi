using FastxWebApi.Models.DTOs;

namespace FastxWebApi.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(TokenUser user);
    }
}
