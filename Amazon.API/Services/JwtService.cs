using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Amazon.API.Model.Domain;
namespace Amazon.API.Services
{
    public class JwtService
    {
        private readonly IConfiguration configuration;

        public JwtService(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {//create secret security key
            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes( //convert text tobytes
                        configuration["Jwt:Key"]!));//get the jwt

            var credentials = //create digital signature use HMAC sha 256 algo
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Name,
                    user.Name),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role)
            };

            var token =
                new JwtSecurityToken(//actual jwt object
                    issuer:
                        configuration["Jwt:Issuer"], 

                    audience:
                        configuration["Jwt:Audience"],

                    claims:
                        claims,

                    expires:
                        DateTime.Now.AddHours(1),

                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}