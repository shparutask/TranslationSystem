using System.Data.SqlClient;
using System;
using System.Text;

namespace QueryExecution
{
    public class QueryExecutor
    {
        private string connectionString;

        public QueryExecutor(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string ExecuteQuery(string query)
        {
            StringBuilder result = new StringBuilder();
            using (SqlConnection sql_conn = new SqlConnection(connectionString))
            {
                sql_conn.Open();
                var keys = sql_conn.CreateCommand();
                keys.CommandText = query;
                SqlDataReader m_keys = null;

                try
                {
                    m_keys = keys.ExecuteReader();
                }
                catch (Exception ex)
                {
                    return $"Sorry, an error while executing query: {ex.Message}";
                }

                int count = 0;
                while (m_keys.Read())
                {
                    count++;
                    for (int i = 0; i < m_keys.FieldCount; i++)
                        result.Append($"{m_keys[i].ToString()}\t");

                    if (count > 1)
                        result.Insert(result.ToString().LastIndexOf('\n') + 1, count.ToString() + ". ");

                    if (count == 2)
                        result.Insert(0, "1. ");

                    result.Append("\n");
                }
            }

            return result.ToString();
        }
    }
}
