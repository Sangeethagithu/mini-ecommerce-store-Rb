using Amazon.API.Models.DTOs.Auth;
using Microsoft.Data.SqlClient;
using System.Data;
using Amazon.API.Services;
using Amazon.API.Model.Domain;
using Microsoft.EntityFrameworkCore;
namespace Amazon.API.Repositories
{
    public class AuthRepository//handle register and login
    {
        private readonly IConfiguration configuration;
        private readonly PasswordService passwordService;
        public AuthRepository(
     IConfiguration configuration,
     PasswordService passwordService)
        {
            this.configuration = configuration;
            this.passwordService = passwordService;
        }


        //for register
        public void Register(RegisterDto dto)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "RegisterUser",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@Id",
                Guid.NewGuid());

            command.Parameters.AddWithValue(
                "@Name",
                dto.Name);

            command.Parameters.AddWithValue(
                "@Email",
                dto.Email);

            string hashedPassword =
                passwordService.HashPassword(
                    dto.Password);

            command.Parameters.AddWithValue(
                "@Password",
                hashedPassword);

            command.Parameters.AddWithValue(
                "@Role",
                "Customer");

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        //existing user email
        public bool EmailExists(string email)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "SELECT COUNT(*) FROM Users WHERE Email = @Email",
                    connection);

            command.Parameters.AddWithValue(
                "@Email",
                email);

            connection.Open();

            int count =
                (int)command.ExecuteScalar();

            connection.Close();

            return count > 0;
        }


        //forget password
        public void ForgotPassword(
    ForgotPasswordDto dto)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "ForgotPassword",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            string hashedPassword =
                passwordService.HashPassword(
                    dto.NewPassword);

            command.Parameters.AddWithValue(
                "@Email",
                dto.Email);

            command.Parameters.AddWithValue(
                "@Password",
                hashedPassword);

            connection.Open();

            int rows =
                command.ExecuteNonQuery();

            connection.Close();

            if (rows == 0)
            {
                throw new Exception(
                    "Email not found");
            }
        }
        //login
        public User? Login(LoginDto dto)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "LoginUser",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@Email",
                dto.Email);

         

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            User? user = null;

            if (reader.Read())
            {

                string storedHash =
    reader["Password"].ToString()!;

                bool isValid =
                    passwordService.VerifyPassword(
                        dto.Password,
                        storedHash);

                if (!isValid)
                {
                    connection.Close();
                    return null;
                }



                user = new User
                {
                    Id = Guid.Parse(
                        reader["Id"].ToString()!),

                    Name =
                        reader["Name"].ToString()!,

                    Email =
                        reader["Email"].ToString()!,

                    Role =
                        reader["Role"].ToString()!
                };
            }

            connection.Close();

            return user;
        }

       // SavingChangesEventArgs refresh token so that communicate with db
        public void SaveRefreshToken(
    Guid userId,
    string refreshToken)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            using SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    @"UPDATE Users
              SET RefreshToken=@RefreshToken,
                  RefreshTokenExpiry=@Expiry
              WHERE Id=@Id",
                    connection);

            command.Parameters.AddWithValue(
                "@RefreshToken",
                refreshToken);

            command.Parameters.AddWithValue(
                "@Expiry",
                DateTime.UtcNow.AddDays(30));

            command.Parameters.AddWithValue(
                "@Id",
                userId);

            connection.Open();

            command.ExecuteNonQuery();
        }
        //find user with help of  refres token 
        public User? GetUserByRefreshToken(
    string refreshToken)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            using SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    @"SELECT *
              FROM Users
              WHERE RefreshToken=@RefreshToken
              AND RefreshTokenExpiry > @Now",
                    connection);

            command.Parameters.AddWithValue(
                "@RefreshToken",
                refreshToken);

            command.Parameters.AddWithValue(
                "@Now",
                DateTime.UtcNow);

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new User
            {
                Id = Guid.Parse(
                    reader["Id"].ToString()!),

                Name =
                    reader["Name"].ToString()!,

                Email =
                    reader["Email"].ToString()!,

                Role =
                    reader["Role"].ToString()!
            };
        }

        //logout user remove refresh token by userid
        public void RemoveRefreshToken(
    Guid userId)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            using SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    @"UPDATE Users
              SET RefreshToken = NULL,
                  RefreshTokenExpiry = NULL
              WHERE Id = @Id",
                    connection);

            command.Parameters.AddWithValue(
                "@Id",
                userId);

            connection.Open();

            command.ExecuteNonQuery();
        }
        //remove refresh token by email
        public void RemoveRefreshTokenByEmail(
    string email)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            using SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    @"UPDATE Users
              SET RefreshToken = NULL,
                  RefreshTokenExpiry = NULL
              WHERE Email = @Email",
                    connection);

            command.Parameters.AddWithValue(
                "@Email",
                email);

            connection.Open();

            command.ExecuteNonQuery();
        }
    }
}