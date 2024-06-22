using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PIEAPI.DataLayer.DataTransferObjects;
using PIEAPI.DataLayer.Repositories;
using PIEAPI.Utility;

namespace PIEAPI.Controllers
{
    [ApiController]
    [Route("stats")]
    public class StatisticsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly StatisticsRepository _statisticsRepository;

        public StatisticsController(IConfiguration config)
        {
            _configuration = config;
            _statisticsRepository = new StatisticsRepository();
        }

        [HttpGet]
        public async Task<List<StatisticQuestionCount>> Get(DateTime dateTime)
        {
            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();

                return _statisticsRepository.GetQuestionCount(dateTime, 0, conn).OrderByDescending(x => x.Count).ToList();

                conn.Close();
            }
        }
    }
}
