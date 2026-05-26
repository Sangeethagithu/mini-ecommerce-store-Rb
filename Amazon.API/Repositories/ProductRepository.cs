
using Amazon.API.Model.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using Amazon.API.Models.DTOs.Product;


namespace Amazon.API.Repositories
{
    public class ProductRepository
    {
        private readonly IConfiguration configuration;

        public ProductRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        //GET operation
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            string connectionString =
                configuration.GetConnectionString("DefaultConnection");

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("GetAllProducts", connection);

            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product
                {
                    Id = Guid.Parse(reader["Id"].ToString()),
                    Name = reader["Name"].ToString(),
                    Description = reader["Description"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"]),
                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                    ImageUrl = reader["ImageUrl"].ToString(),
                    CategoryId = Guid.Parse(reader["CategoryId"].ToString())
                };

                products.Add(product);
            }

            connection.Close();

            return products;
        }

        //POST operation
        public void AddProduct(CreateProductDto dto)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("AddProduct", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", Guid.NewGuid());

            command.Parameters.AddWithValue("@Name", dto.Name);

            command.Parameters.AddWithValue("@Description", dto.Description);

            command.Parameters.AddWithValue("@Price", dto.Price);

            command.Parameters.AddWithValue("@StockQuantity", dto.StockQuantity);

            command.Parameters.AddWithValue("@ImageUrl", dto.ImageUrl);

            command.Parameters.AddWithValue("@CategoryId", dto.CategoryId);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        // PUT operation
        public void UpdateProduct(Guid id, UpdateProductDto dto)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("UpdateProduct", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            command.Parameters.AddWithValue("@Name", dto.Name);

            command.Parameters.AddWithValue("@Description", dto.Description);

            command.Parameters.AddWithValue("@Price", dto.Price);

            command.Parameters.AddWithValue("@StockQuantity", dto.StockQuantity);

            command.Parameters.AddWithValue("@ImageUrl", dto.ImageUrl);

            command.Parameters.AddWithValue("@CategoryId", dto.CategoryId);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }


        //delete operation
        public void DeleteProduct(Guid id)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("DeleteProduct", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        //search product product listing
        public List<Product> SearchProducts(string name)
        {
            List<Product> products = new List<Product>();

            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("SearchProducts", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Name", name);

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString()!,
                    Description = reader["Description"].ToString()!,
                    Price = Convert.ToDecimal(reader["Price"]),
                    StockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                    ImageUrl = reader["ImageUrl"].ToString()!,
                    CategoryId = Guid.Parse(reader["CategoryId"].ToString()!)
                };

                products.Add(product);
            }

            connection.Close();

            return products;
        }
    }
}
