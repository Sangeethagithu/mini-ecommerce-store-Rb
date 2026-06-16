namespace Amazon.API.Models.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }//after login get tokens

        public string RefreshToken { get; set; }
    }
}