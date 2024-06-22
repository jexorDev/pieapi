using Npgsql;
using PIEAPI.DataLayer.DataTransferObjects;

namespace PIEAPI.DataLayer.Repositories
{
    public class QuestionsRepository
    {
        public List<Question> GetQuestions(int userId, NpgsqlConnection conn)
        {
            const string Sql = @"
SELECT
 id
,category_id
,text
,created_by
FROM 
    questions
WHERE 
    created_by IS NULL 
OR
    created_by = @created_by
";
            List<Question> questions = new List<Question>();

            using (var cmd = new NpgsqlCommand(Sql, conn))
            {
                cmd.Parameters.AddWithValue("created_by", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        questions.Add(new Question
                        {
                            Id = int.Parse(reader["id"].ToString()),
                            CategoryId = int.Parse(reader["category_id"].ToString()),
                            Text = reader["text"].ToString(),
                            CreatedBy = reader["created_by"] == DBNull.Value ? null : int.Parse(reader["created_by"].ToString())
                        });
                    }

                }
            }

            return questions;
        }

        public int InsertQuestion(NpgsqlConnection conn, NpgsqlTransaction trans, Question question)
        {
            const string Sql = @"
INSERT INTO questions 
(
 category_id
,text
,created_by
)
VALUES
(
 @category_id
,@text
,@created_by
) RETURNING id
";
            using (var cmd = new NpgsqlCommand(Sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("category_id", question.CategoryId);
                cmd.Parameters.AddWithValue("text", question.Text);
                cmd.Parameters.AddWithValue("created_by", question.CreatedBy);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
