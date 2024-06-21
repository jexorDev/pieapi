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
        public async Task<List<Entry>> Get()
        {
            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();

                var entries = _entriesRepository.GetEntries(DateTime.Now, 0, conn);
                entries.ForEach(entry => entry.Timestamp = entry.Timestamp.ToLocalTime());
                return entries;

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
    }
}
