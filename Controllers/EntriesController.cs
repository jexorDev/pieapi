using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PIEAPI.Controllers.Models;
using PIEAPI.DataLayer.DataTransferObjects;
using PIEAPI.DataLayer.Repositories;
using PIEAPI.Utility;

namespace PIEAPI.Controllers
{
    [ApiController]
    [Route("entries")]
    public class EntriesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly EntriesRepository _entriesRepository;

        public EntriesController(IConfiguration config)
        {
            _configuration = config;
            _entriesRepository = new EntriesRepository();
        }

        [HttpGet]
        public async Task<List<Entry>> Get(DateTime dateTime)
        {
            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();

                return _entriesRepository.GetEntries(dateTime, 0, conn);

                conn.Close();
            }
        }

        [HttpPost]
        public async Task<Entry> Create(EntryCreateModel model)
        {
            var newEntry = new Entry
            {
                QuestionId = model.QuestionId,
                EnteredBy = 0,
                LocationId = model.LocationId,
                Timestamp = DateTime.Parse(model.Timestamp),
            };

            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();

                var id = _entriesRepository.InsertEntry(conn, transaction, newEntry);
                newEntry.Id = id;
                newEntry.Timestamp = DateTime.Now;

                transaction.Commit();
                conn.Close();
            }

            return newEntry;
        }

        [HttpPut]
        public async Task Update(EntryUpdateModel model)
        {
            var entry = new Entry
            {
                Id = model.Id,
                LocationId = model.LocationId
            };

            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();

                _entriesRepository.UpdateEntry(conn, transaction, entry);
                
                transaction.Commit();
                conn.Close();
            }
        }

        [HttpDelete("{*id}")]
        public async Task Delete([FromRoute]int id)
        {
            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();

                _entriesRepository.DeleteEntry(conn, transaction, id);

                transaction.Commit();
                conn.Close();
            }
        }
    }
}
