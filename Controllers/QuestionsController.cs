using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PIEAPI.Controllers.Models;
using PIEAPI.DataLayer.DataTransferObjects;
using PIEAPI.DataLayer.Repositories;
using PIEAPI.Utility;

namespace PIEAPI.Controllers
{
    [ApiController]
    [Route("questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly QuestionsRepository _questionsRepository;

        public QuestionsController(IConfiguration config)
        {
            _configuration = config;
            _questionsRepository = new QuestionsRepository();
        }

        [HttpGet]
        public async Task<List<Question>> Get()
        {
            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();

                return _questionsRepository.GetQuestions(0, conn);

                conn.Close();
            }
        }

        [HttpPost]
        public async Task<Question> Create(QuestionCreateModel model)
        {
            var newQuestion = new Question
            {
                Text = model.Text,
                CategoryId = model.CategoryId,
                CreatedBy = 0
            };

            using (var conn = new NpgsqlConnection(DatabaseConnectionStringBuilder.GetSqlConnectionString(_configuration)))
            {
                conn.Open();
                NpgsqlTransaction transaction = conn.BeginTransaction();

                var id = _questionsRepository.InsertQuestion(conn, transaction, newQuestion);
                newQuestion.Id = id;

                transaction.Commit();
                conn.Close();
            }

            return newQuestion;
        }
    }
}
