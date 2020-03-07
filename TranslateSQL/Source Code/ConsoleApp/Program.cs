using System;
using TranslateSQL;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите название базы:");
            string dbName = Console.ReadLine();
            Console.WriteLine("Введите название экземпляра сервера базы:");
            string serverName = Console.ReadLine();
            Console.WriteLine("Введите вопрос:");
            string question = Console.ReadLine();

            string sqlQuery = SQLQueryBuilder.Build(question);
            string result = new QueryExecution(dbName, serverName).ExecuteQuery(sqlQuery);
            Console.WriteLine(result);
            Console.ReadKey();
        }
    }
}
