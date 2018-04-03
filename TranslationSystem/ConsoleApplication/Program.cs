using TranslationLib;
using System;
using System.Data.SqlClient;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Translation t = new Translation();
            string question = "What is the e-mail of Andrey";
            string query = t.ToQuery(question);
            using (SqlConnection sql_conn = new SqlConnection(@"Data Source = SOPHIESHPA\SQLEXPRESS; Initial Catalog = MIGRATION_EXPERT; Integrated Security = True"))
            {
                sql_conn.Open();
                var keys = sql_conn.CreateCommand();
                keys.CommandText = query;
                var m_keys = keys.ExecuteReader();
                while (m_keys.Read())
                {
                    Console.WriteLine(m_keys[0].ToString());
                }
            }

            question = "What is the name of author of publication like About Me";//"What is the e-mail of Andrey";
            query = t.ToQuery(question);
            using (SqlConnection sql_conn = new SqlConnection(@"Data Source = SOPHIESHPA\SQLEXPRESS; Initial Catalog = MIGRATION_EXPERT; Integrated Security = True"))
            {
                sql_conn.Open();
                var keys = sql_conn.CreateCommand();
                keys.CommandText = query;
                var m_keys = keys.ExecuteReader();
                while (m_keys.Read())
                {
                    Console.WriteLine(m_keys[0].ToString());
                }
            }

            Console.ReadKey();
        }
    }
}
