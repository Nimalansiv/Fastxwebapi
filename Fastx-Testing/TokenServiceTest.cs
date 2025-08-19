using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FastxWebApi.Interfaces;
using FastxWebApi.Models.DTOs;
using FastxWebApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace FastxTest
{
    internal class TokenServiceTest
    {
        private ITokenService _tokenService;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Tokens:JWT"] = "ThisIsASecretKeyForTestingAndShouldBeLongerThan32Characters"
                }).Build();

            _tokenService = new TokenService(config);
        }

        [Test]
        public async Task TokenServicePassTest()
        {
           
            var user = new TokenUser
            {
                Username = "Test",
                Role = "Tester"
            };
            
            var result = await _tokenService.GenerateToken(user);

           
            Assert.That(result, Is.Not.Null);


        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}