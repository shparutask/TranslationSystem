using System;
using TranslateSQL;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string question = Console.ReadLine();
            string sqlQuery = SQLQueryBuilder.Build(question);
            string result = new QueryExecution("SPB_ATTRACTIONS", "SOPHIESHPA").ExecuteQuery(sqlQuery);
        }
    }
}
