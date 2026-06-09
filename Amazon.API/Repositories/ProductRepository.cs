
using Amazon.API.Model.Domain;
using Microsoft.Data.SqlClient;
using System.Data;
using Amazon.API.Models.DTOs.Product;
using Microsoft.AspNetCore.Hosting;

namespace Amazon.API.Repositories
{
    public class ProductRepository
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public ProductRepository(
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            this.configuration = configuration;

            this.environment = environment;
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

            string fileName =
    Guid.NewGuid().ToString() +
    Path.GetExtension(
        dto.Image.FileName);

            string imagePath =
                Path.Combine(
                    environment.ContentRootPath,
                    "Images",
                    fileName);

            using (FileStream stream =
                new FileStream(
                    imagePath,
                    FileMode.Create))
            {
                dto.Image.CopyTo(stream);
            }


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

            command.Parameters.AddWithValue(
    "@ImageUrl",
    fileName);

            command.Parameters.AddWithValue("@CategoryId", dto.CategoryId);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
        //prod
        public Product GetProductById(Guid id)
        {
            Product product = null;

            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "GetProductById",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@Id",
                id);

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            if (reader.Read())
            {
                product = new Product
                {
                    Id = Guid.Parse(
                        reader["Id"].ToString()!),

                    Name =
                        reader["Name"].ToString()!,

                    Description =
                        reader["Description"].ToString()!,

                    Price =
                        Convert.ToDecimal(
                            reader["Price"]),

                    StockQuantity =
                        Convert.ToInt32(
                            reader["StockQuantity"]),

                    ImageUrl =
                        reader["ImageUrl"].ToString()!,

                    CategoryId =
                        Guid.Parse(
                            reader["CategoryId"].ToString()!)
                };
            }

            connection.Close();

            return product;
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
            if (products.Count == 0)
            {
                var allProducts =
                    GetAllProducts();

                products =
                    allProducts
                    .Where(p =>

                        p.Name
                         .ToLower()
                         .Contains(
                             name.ToLower())

                        ||

                        p.Name
                         .ToLower()
                         .Split(' ')
                         .Any(word =>

                             LevenshteinDistance(
                                 word,
                                 name.ToLower()) <= 2)

                    )
                    .ToList();
            }

            return products;
        }

        //updating the stock restocking
        public void UpdateStock(
    UpdateStockDto dto)
        {
            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "UpdateProductStock",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@ProductId",
                dto.ProductId);

            command.Parameters.AddWithValue(
                "@StockQuantity",
                dto.StockQuantity);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        //low stock allert
        public List<LowStockProductDto>
    GetLowStockProducts()
        {
            List<LowStockProductDto> products =
                new List<LowStockProductDto>();

            string connectionString =
                configuration.GetConnectionString(
                    "DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand(
                    "GetLowStockProducts",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                LowStockProductDto product =
                    new LowStockProductDto
                    {
                        Id = Guid.Parse(
                            reader["Id"].ToString()!),

                        Name =
                            reader["Name"].ToString()!,

                        StockQuantity =
                            Convert.ToInt32(
                                reader["StockQuantity"])
                    };

                products.Add(product);
            }

            connection.Close();

            return products;
        }


        private int LevenshteinDistance(
    string s,
    string t)
        {
            int[,] d =
                new int[
                    s.Length + 1,
                    t.Length + 1];

            for (int i = 0; i <= s.Length; i++)
                d[i, 0] = i;

            for (int j = 0; j <= t.Length; j++)
                d[0, j] = j;

            for (int i = 1; i <= s.Length; i++)
            {
                for (int j = 1; j <= t.Length; j++)
                {
                    int cost =
                        s[i - 1] == t[j - 1]
                        ? 0
                        : 1;

                    d[i, j] =
                        Math.Min(
                            Math.Min(
                                d[i - 1, j] + 1,
                                d[i, j - 1] + 1),
                            d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }
    }
}
