using TranslationLib;
using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Translation();
            var q = new QueryExecution();
            List<string> queries = new List<string> {
                                                     "What is the mail of Andrey", "What is the degree of Andrey",
                                                     "What is the mail of expert who has a publication like my life", "What is the degree of expert who has a publication about my life",
                                                     "What is the mail of expert of publication like my life", "What is the degree of expert of publication about my life",
                                                     "What is the mail of author of publications with title like me", "What is the degree of author of publications with title like me",
                                                     "What is the mail of author of publications which has a description like my life", "What is the degree of author of publications which has a references like Natural",
                                                     "Who has the mail like sofish718@sdf.ru", "Who has the degree like Кандидат наук",
                                                     "Whose mail is sofish718@sdf.ru", "Whose degree is Кандидат наук",
                                                     "What is the mail of expert who has a publication which has a description like my life",
                                                     "What is the degree of expert who has a publication which has a description like my life"
                                                    };

            string result = "";

            foreach (var s in queries)
            {
                result = t.ToQuery(s);
                Console.WriteLine("\n" + q.ExecuteQuery(result) + "\n" + result);
            }

            Console.WriteLine("Success!");
            Console.ReadKey();
        }
    }
}
