using System.Data.SqlClient;
using System;

namespace TranslationLib
{
    public class QueryExecution
    {
        public string Result { get; set; }
        public string ResultQuery { get; set; }
        string db, server;

        public QueryExecution(string db, string server)
        {
            this.db = db;
            this.server = server;
        }

        public void ExecuteQuery(string question, string language)
        {
            YandexTranslator yt = new YandexTranslator();
            if (question.IndexOf('?') == -1)
                question += "?";
            if (language == "Русский")
                question = yt.Translate(question, "ru-en");
            string connString = @"Data Source = .\" + server + "; Initial Catalog = " + db + "; Integrated Security = True";
            Translation t = new Translation(connString);

            string query = t.ToQuery(question);
            if (query == "err")
            {
                Result = "Sorry, an error while executing query";
                return;
            }

            string result = "";
            using (SqlConnection sql_conn = new SqlConnection(connString))
            {
                ResultQuery = query;
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
                    Result = "Sorry, an error while executing query: " + ex.Message;
                    return;
                }

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
            Result = result.Substring(0, result.Length);
        }
    }
}
