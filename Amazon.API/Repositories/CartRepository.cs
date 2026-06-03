using Amazon.API.Models.DTOs.Cart;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Amazon.API.Repositories
{
    public class CartRepository
    {
        private readonly IConfiguration configuration;


        public CartRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public Guid? GetCartIdByEmail(string email) //view cart using email
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

        //cart creation using api automatically
        public Guid GetOrCreateCart(string email)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            connection.Open();

            SqlCommand findCommand =
                new SqlCommand(
                    "SELECT Id FROM Carts WHERE UserEmail = @Email",
                    connection);

            findCommand.Parameters.AddWithValue(
                "@Email",
                email);

            object result =
                findCommand.ExecuteScalar();

            if (result != null)
            {
                Guid cartId =
                    Guid.Parse(result.ToString()!);

                connection.Close();

                return cartId;
            }

            Guid newCartId =
                Guid.NewGuid();

            SqlCommand createCommand =
                new SqlCommand(
                    @"INSERT INTO Carts
              (Id, UserEmail)
              VALUES
              (@Id, @Email)",
                    connection);

            createCommand.Parameters.AddWithValue(
                "@Id",
                newCartId);

            createCommand.Parameters.AddWithValue(
                "@Email",
                email);

            createCommand.ExecuteNonQuery();

            connection.Close();

            return newCartId;
        }

        //add to cart
        public void AddToCart(
            AddToCartDto dto,
            string email)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            Guid cartId =
    GetOrCreateCart(email);

            SqlCommand command =
                new SqlCommand("AddToCart", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", Guid.NewGuid());

            command.Parameters.AddWithValue(
     "@CartId",
     cartId);

            command.Parameters.AddWithValue("@ProductId", dto.ProductId);

            command.Parameters.AddWithValue("@Quantity", dto.Quantity);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        //get cart items
        public List<CartItemResponseDto> GetCartItems(Guid cartId)
        {
            List<CartItemResponseDto> items =
                new List<CartItemResponseDto>();

            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("GetCartItems", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CartId", cartId);

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                CartItemResponseDto item =
                    new CartItemResponseDto
                    {
                        ProductId =
                            Guid.Parse(reader["ProductId"].ToString()!),

                        ProductName =
                            reader["ProductName"].ToString()!,

                        Price =
                            Convert.ToDecimal(reader["Price"]),

                        Quantity =
                            Convert.ToInt32(reader["Quantity"]),

                        ImageUrl =
                            reader["ImageUrl"].ToString()!
                    };

                items.Add(item);
            }

            connection.Close();

            return items;
        }


        //update cart item quantity
        public void UpdateCartQuantity(UpdateCartQuantityDto dto)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("UpdateCartQuantity", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CartItemId",
                dto.CartItemId);

            command.Parameters.AddWithValue(
                "@Quantity",
                dto.Quantity);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
        //remove item form cart
        public void RemoveCartItem(Guid cartItemId)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("RemoveCartItem", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CartItemId",
                cartItemId);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
        //get total amt in cart
        public decimal GetCartTotal(Guid cartId)
        {
            decimal total = 0;

            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("GetCartTotal", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CartId", cartId);

            connection.Open();

            object result = command.ExecuteScalar();

            if (result != DBNull.Value)
            {
                total = Convert.ToDecimal(result);
            }

            connection.Close();

            return total;
        }
    }
}