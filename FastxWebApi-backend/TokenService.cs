<<<<<<< HEAD:FastxWebApi-backend/TokenService.cs
﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace FastxWebApi.Services
{
    public class TokenService:ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            string secret = configuration["Tokens:JWT"] ?? "";
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
        public async Task<string> GenerateToken(TokenUser user)
        {
           
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.Role),
                

            };
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
           
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = creds,


            };
            var tokenHandler = new JwtSecurityTokenHandler();
           
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }



    }
}
=======
﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace FastxWebApi.Services
{
    public class TokenService:ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration configuration)
        {
            string secret = configuration["Tokens:JWT"] ?? "";
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
        public async Task<string> GenerateToken(TokenUser user)
        {
           
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.Role),
                

            };
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
            }

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
           
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = creds,


            };
            var tokenHandler = new JwtSecurityTokenHandler();
           
            var token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }



    }
}
>>>>>>> e40ecec (initial commit - backend fastx):Services/TokenService.cs
