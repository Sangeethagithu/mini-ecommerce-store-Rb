using Amazon.API.Models.DTOs.Auth;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Amazon.API.Services;
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

        //register a new user
        [HttpPost("register")]
        public IActionResult Register(
    RegisterDto dto)
        {
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