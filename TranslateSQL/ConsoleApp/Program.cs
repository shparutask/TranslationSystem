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
            string result = new QueryExecution("SPB_ATTRACTIONS", "SQLEXPRESS").ExecuteQuery(sqlQuery);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
