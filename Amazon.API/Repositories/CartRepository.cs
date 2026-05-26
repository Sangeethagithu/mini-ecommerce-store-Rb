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

        //add to cart
        public void AddToCart(AddToCartDto dto)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("AddToCart", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", Guid.NewGuid());

            command.Parameters.AddWithValue("@CartId", dto.CartId);

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