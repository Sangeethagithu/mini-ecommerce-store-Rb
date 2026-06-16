using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Amazon.API.Model.Domain;
using System.Security.Cryptography;
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
                        
    DateTime.UtcNow.AddMinutes(15),//usd utcnow to avoid timezone issues shortlived acces token only 15mins

                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }




        // Generate a secure random key for JWT signing-refresh token 
        public string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];//array of 64 bytes created

            using var rng =
                RandomNumberGenerator.Create();
            //crytographically secure random number generator-unpredictable value created
            //os randomness not any algo like bcrypt
            rng.GetBytes(bytes);//fill that array with 64bytes

            return Convert.ToBase64String(bytes);//convert to string
        }
    }
}