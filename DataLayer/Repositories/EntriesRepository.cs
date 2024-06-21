using Npgsql;
using PIEAPI.DataLayer.DataTransferObjects;

namespace PIEAPI.DataLayer.Repositories
{
    public class EntriesRepository
    {
        public List<Entry> GetEntries(DateTime dateFilter, int userId, NpgsqlConnection conn)
        {
            const string Sql = @"
SELECT
 question_id
,timestamp
,entered_by
,location_id
FROM entries
WHERE 
    entered_by = @entered_by
AND
    timestamp::date = @timestamp::date
";
            List<Entry> entries = new List<Entry>();

            using (var cmd = new NpgsqlCommand(Sql, conn))
            {
                cmd.Parameters.AddWithValue("entered_by", userId);
                cmd.Parameters.AddWithValue("timestamp", dateFilter);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entries.Add(new Entry
                        {
                            QuestionId = int.Parse(reader["question_id"].ToString()),
                            Timestamp = DateTime.Parse(reader["timestamp"].ToString()),
                            EnteredBy = int.Parse(reader["entered_by"].ToString()),
                            LocationId = short.Parse(reader["location_id"].ToString())
                        });
                    }

                }
            }

            return entries;
        }

        public int InsertEntry(NpgsqlConnection conn, NpgsqlTransaction trans, Entry entry)
        {
            const string Sql = @"
INSERT INTO entries 
(
 question_id
,timestamp
,entered_by
,location_id
)
VALUES
(
 @question_id
,CURRENT_TIMESTAMP
,0
,@location_id
) RETURNING id
";
            using (var cmd = new NpgsqlCommand(Sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("question_id", entry.QuestionId);
                cmd.Parameters.AddWithValue("location_id", entry.LocationId);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}
