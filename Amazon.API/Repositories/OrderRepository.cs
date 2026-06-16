using Amazon.API.Model.Domain;
using Amazon.API.Models.DTOs.Order;
using Microsoft.Data.SqlClient;
using System.Data;
using Amazon.API.Models.DTOs.Dashboard;
namespace Amazon.API.Repositories
{
    public class OrderRepository
    {
        private readonly IConfiguration configuration;

        public OrderRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        //new style so only that user get his cart
        public Guid? GetCartIdByEmail(string email)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "SELECT Id FROM Carts WHERE UserEmail = @Email",
                    connection);

            command.Parameters.AddWithValue(
                "@Email",
                email);

            connection.Open();

            object result =
                command.ExecuteScalar();

            connection.Close();

            if (result == null)
            {
                return null;
            }

            return Guid.Parse(result.ToString()!);
        }
        //admindashboard
        public DashboardDto GetDashboardStats()
        {
            DashboardDto dashboard =
                new DashboardDto();

            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "GetDashboardStats",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            if (reader.Read())
            {
                dashboard.TotalProducts =
                    Convert.ToInt32(
                        reader["TotalProducts"]);

                dashboard.TotalOrders =
                    Convert.ToInt32(
                        reader["TotalOrders"]);

                dashboard.TotalRevenue =
                    Convert.ToDecimal(
                        reader["TotalRevenue"]);
            }

            connection.Close();

            return dashboard;
        }

        //recent prod
        public List<Order> GetRecentOrders()
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
                    "GetRecentOrders",
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
        //create order
        public void Checkout(string email)
        {

            Guid? cartId =
    GetCartIdByEmail(email);

            if (cartId == null)
            {
                throw new Exception(
                    "Cart not found");
            }




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
                cartId.Value);

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
                cartId.Value);

            command.Parameters.AddWithValue(
                "@TotalAmount",
                total);

            command.ExecuteNonQuery();

            connection.Close();
        }
        //get prev orders of user
        public List<Order> GetAllOrders(string email)
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
            command.Parameters.AddWithValue(
    "@Email",
    email);
            

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

        //admin getorder
        public List<Order> GetAllOrdersForAdmin()
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
                    "GetAllOrdersForAdmin",
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