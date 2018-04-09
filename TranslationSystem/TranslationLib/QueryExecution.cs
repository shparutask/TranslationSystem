using System.Data.SqlClient;

namespace TranslationLib
{
    public class QueryExecution
    {
        public string ExecuteQuery(string query)
        {
            string result = "";
            using (SqlConnection sql_conn = new SqlConnection(@"Data Source = SOPHIESHPA\SQLEXPRESS; Initial Catalog = MIGRATION_EXPERT; Integrated Security = True"))
            {

                sql_conn.Open();
                var keys = sql_conn.CreateCommand();
                keys.CommandText = query;
                var m_keys = keys.ExecuteReader();
                while (m_keys.Read())
                {
                    result += m_keys[0].ToString() + " ";
                }
            }
            return result;
        }
    }
}
