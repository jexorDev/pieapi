using Npgsql;
using PIEAPI.DataLayer.DataTransferObjects;

namespace PIEAPI.DataLayer.Repositories
{
    public class StatisticsRepository
    {
        public List<StatisticQuestionCount> GetQuestionCount(DateTime dateFilter, int userId, NpgsqlConnection conn)
        {
            const string Sql = @"
SELECT
 COUNT(*) count
,question_id
,text
,categories.id category_id
,categories.name category_name
FROM 
    entries
INNER JOIN 
    questions
ON
    questions.id = question_id
INNER JOIN
    categories
ON
    categories.id = questions.category_id
WHERE 
    entered_by = @entered_by
AND
    timestamp::date = @timestamp::date
GROUP BY
     question_id
    ,text
    ,categories.id 
    ,categories.name 
";
            List<StatisticQuestionCount> stats = new List<StatisticQuestionCount>();

            using (var cmd = new NpgsqlCommand(Sql, conn))
            {
                cmd.Parameters.AddWithValue("entered_by", userId);
                cmd.Parameters.AddWithValue("timestamp", dateFilter);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stats.Add(new StatisticQuestionCount
                        {
                            Count = int.Parse(reader["count"].ToString()),
                            QuestionId = int.Parse(reader["question_id"].ToString()),
                            QuestionText = reader["text"].ToString(),
                            CategoryId = int.Parse(reader["category_id"].ToString()),
                            CategoryName = reader["category_name"].ToString(),
                        });
                    }

                }
            }

            return stats;
        }

    }
}
