using System.Data.SqlClient;

namespace TranslationLib
{
    public class QueryExecution
    {
        public string Result { get; set; }
        public string ResultQuery { get; set; }

        public void ExecuteQuery(string query)
        {
            string result = "";
            using (SqlConnection sql_conn = new SqlConnection(@"Data Source = SOPHIESHPA\SQLEXPRESS; Initial Catalog = MIGRATION_EXPERT; Integrated Security = True"))
            {
                ResultQuery = query;
                sql_conn.Open();
                var keys = sql_conn.CreateCommand();
                keys.CommandText = query;
                var m_keys = keys.ExecuteReader();
                while (m_keys.Read())
                {
                    for (int i = 0; i < m_keys.FieldCount; i++)
                        result += m_keys[i].ToString() + "\n";
                }
            }
            Result = result;
        }
    }
}
