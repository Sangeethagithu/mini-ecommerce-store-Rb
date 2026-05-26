

using Amazon.API.Model.Domain;
using Amazon.API.Models.DTOs.Category;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Amazon.API.Repositories
{
    public class CategoryRepository
    {
        private readonly IConfiguration configuration;

        public CategoryRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<Category> GetAllCategories()
        {
            List<Category> categories = new List<Category>();

            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("GetAllCategories", connection);

            command.CommandType = CommandType.StoredProcedure;

            connection.Open();

            SqlDataReader reader =
                command.ExecuteReader();

            while (reader.Read())
            {
                Category category = new Category
                {
                    Id = Guid.Parse(reader["Id"].ToString()!),
                    Name = reader["Name"].ToString()!
                };

                categories.Add(category);
            }

            connection.Close();

            return categories;
        }

        public void AddCategory(CreateCategoryDto dto)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("AddCategory", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", Guid.NewGuid());

            command.Parameters.AddWithValue("@Name", dto.Name);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        //update category
        public void UpdateCategory(Guid id, UpdateCategoryDto dto)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("UpdateCategory", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            command.Parameters.AddWithValue("@Name", dto.Name);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }

        //delete category

        public void DeleteCategory(Guid id)
        {
            string connectionString =
                configuration.GetConnectionString("DefaultConnection")!;

            SqlConnection connection =
                new SqlConnection(connectionString);

            SqlCommand command =
                new SqlCommand("DeleteCategory", connection);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Id", id);

            connection.Open();

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}