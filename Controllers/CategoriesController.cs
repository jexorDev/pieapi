using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PIEAPI.Controllers.Models;
using PIEAPI.DataLayer.DataTransferObjects;
using PIEAPI.DataLayer.Repositories;
using PIEAPI.Utility;

namespace PIEAPI.Controllers
{
    [ApiController]
    [Route("categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CategoriesRepository _categoriesRepository;

        public CategoriesController(IConfiguration config)
        {
            _configuration = config;
            _categoriesRepository = new CategoriesRepository();
        }

        [HttpGet]
        public async Task<List<Category>> Get()
        {
            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();

                return _categoriesRepository.GetCategories(conn);

                conn.Close();
            }
        }

        [HttpPost]
        public async Task<Category> Create(string name)
        {
            var newCategory = new Category
            {
                Name = name
            };

            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();

                var id = _categoriesRepository.InsertCategory(conn, transaction, newCategory);
                newCategory.Id = id;

                transaction.Commit();
                conn.Close();
            }

            return newCategory;
        }
    }
}
