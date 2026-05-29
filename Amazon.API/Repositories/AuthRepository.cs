using Amazon.API.Models.DTOs.Auth;
using Microsoft.Data.SqlClient;
using System.Data;
using Amazon.API.Services;
using Amazon.API.Model.Domain;
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
    }
}