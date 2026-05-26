using Amazon.API.Models.DTOs.Order;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Amazon.API.Repositories
{
    public class OrderRepository
    {
        private readonly IConfiguration configuration;

        public OrderRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        //create order
        public void Checkout(CheckoutDto dto)
        {
            decimal total = 0;

            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand totalCommand =
                new SqlCommand("GetCartTotal", connection);

            totalCommand.CommandType =
                CommandType.StoredProcedure;

            totalCommand.Parameters.AddWithValue(
                "@CartId",
                dto.CartId);

            connection.Open();

            object result =
                totalCommand.ExecuteScalar();

            if (result != DBNull.Value)
            {
                total =
                    Convert.ToDecimal(result);
            }

            SqlCommand command =
                new SqlCommand("CreateOrder", connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@OrderId",
                Guid.NewGuid());

            command.Parameters.AddWithValue(
                "@CartId",
                dto.CartId);

            command.Parameters.AddWithValue(
                "@TotalAmount",
                total);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}