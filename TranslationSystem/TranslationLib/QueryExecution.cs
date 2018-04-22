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

                int count = 0;
                while (m_keys.Read())
                {
                    count++;
                    for (int i = 0; i < m_keys.FieldCount; i++)
                        result += m_keys[i].ToString() + "\t";
                    if (count > 1) result = result.Insert(result.LastIndexOf('\n') + 1, count.ToString() + ". ");
                    result.Substring(0, result.Length - 1);
                    if (count == 2) result = "1. " + result;
                    result += "\n";
                }
            }
            Result = result.Substring(0, result.Length - 1);
        }
    }
}
