using Npgsql;
using PIEAPI.DataLayer.DataTransferObjects;

namespace PIEAPI.DataLayer.Repositories
{
    public class CategoriesRepository
    {
        public List<Category> GetCategories(NpgsqlConnection conn)
        {
            const string Sql = @"
SELECT
     id
    ,name
FROM 
    categories
";
            List<Category> categories = new List<Category>();

            using (var cmd = new NpgsqlCommand(Sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        Id = int.Parse(reader["id"].ToString()),
                        Name = reader["name"].ToString()
                    });
                }

            }

            return categories;
        }

        public int InsertCategory(NpgsqlConnection conn, NpgsqlTransaction trans, Category category)
        {
            const string Sql = @"
INSERT INTO categories 
(
 name
)
VALUES
(
 @name
) RETURNING id
";
            using (var cmd = new NpgsqlCommand(Sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("name", category.Name);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
