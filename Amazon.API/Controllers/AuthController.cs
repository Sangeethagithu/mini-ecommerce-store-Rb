using Amazon.API.Models.DTOs.Auth;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Amazon.API.Services;
using System.Linq;
namespace Amazon.API.Controllers
{
    [ApiController] //tell its wed api controller
    [Route("api/[controller]")]
    public class AuthController
        : ControllerBase
    {
        private readonly AuthRepository
            authRepository;

        //generate JWT token for authenticated user
        private readonly JwtService jwtService;

        public AuthController(
            AuthRepository authRepository,
            JwtService jwtService)
        {
            this.authRepository =
                authRepository;

            this.jwtService =
                jwtService;
        }
        //forget password
        [HttpPut("forgot-password")]
        public IActionResult ForgotPassword(
     ForgotPasswordDto dto)
        {
            if (dto.NewPassword.Length < 8)
            {
                return BadRequest(
                    "Password must be at least 8 characters");
            }

            if (!dto.NewPassword.Any(char.IsUpper))
            {
                return BadRequest(
                    "Password must contain uppercase letter");
            }

            if (!dto.NewPassword.Any(char.IsLower))
            {
                return BadRequest(
                    "Password must contain lowercase letter");
            }

            if (!dto.NewPassword.Any(char.IsDigit))
            {
                return BadRequest(
                    "Password must contain number");
            }

            try
            {
                authRepository
                    .ForgotPassword(dto);

                return Ok(
                    "Password updated successfully");
            }
            catch
            {
                return BadRequest(
                    "Email not found");
            }
        }
        //register a new user


        //exisring email
        [HttpPost("register")]
        public IActionResult Register(
    RegisterDto dto)
        {
            if (authRepository.EmailExists(dto.Email))
            {
                return BadRequest(
                    "Email already exists");
            }
            if (dto.Password.Length < 8)
            {
                return BadRequest(
                    "Password must be at least 8 characters");
            }

            if (!dto.Password.Any(char.IsUpper))
            {
                return BadRequest(
                    "Password must contain uppercase letter");
            }

            if (!dto.Password.Any(char.IsDigit))
            {
                return BadRequest(
                    "Password must contain number");
            }
            authRepository.Register(dto);

            return Ok(
                "User registered successfully");
        }







        //login user
        [HttpPost("login")]
        public IActionResult Login(
    LoginDto dto)
        {
            var user =
                authRepository.Login(dto);

            if (user == null)
            {
                return BadRequest(
                    "Invalid email or password");
            }

            string token =
              jwtService.GenerateToken(user);

            return Ok(token);
        }

 
    }
}