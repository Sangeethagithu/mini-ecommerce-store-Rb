using Amazon.API.Model.Domain;
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
        //get prev orders of user
        public List<Order> GetAllOrders()
        {
            List<Order> orders =
                new List<Order>();

            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "GetAllOrders",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                Order order = new Order
                {
                    Id = Guid.Parse(
                        reader["Id"].ToString()!),

                    CartId = Guid.Parse(
                        reader["CartId"].ToString()!),

                    TotalAmount = Convert.ToDecimal(
                        reader["TotalAmount"]),

                    OrderDate = Convert.ToDateTime(
                        reader["OrderDate"]),

                    Status = reader["Status"].ToString()!
                };

                orders.Add(order);
            }

            connection.Close();

            return orders;
        }
        //show products in order
        public List<OrderItemDto> GetOrderDetails(
    Guid orderId)
        {
            List<OrderItemDto> items =
                new List<OrderItemDto>();

            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "GetOrderDetails",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@OrderId",
                orderId);

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                OrderItemDto item =
                    new OrderItemDto
                    {
                        ProductName =
                            reader["ProductName"]
                            .ToString()!,

                        Quantity =
                            Convert.ToInt32(
                                reader["Quantity"]),

                        Price =
                            Convert.ToDecimal(
                                reader["Price"])
                    };

                items.Add(item);
            }

            connection.Close();

            return items;
        }
        //order status update
        public void UpdateOrderStatus(
    UpdateOrderStatusDto dto)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "UpdateOrderStatus",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@OrderId",
                dto.OrderId);

            command.Parameters.AddWithValue(
                "@Status",
                dto.Status);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

    }
}