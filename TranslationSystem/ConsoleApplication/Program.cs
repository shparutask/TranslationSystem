using System;
using TranslationLib;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Translation t = new Translation();
             var s = new Lexicon(new dbGraph(@"Data Source = SOPHIESHPA\SQLEXPRESS; Initial Catalog = MIGRATION_EXPERT; Integrated Security = True"));

             t.Tagging_Cat("What is the duck?", s);*/

            Grammar g = new Grammar();
            Console.WriteLine(g.verb_stem("does"));
            Console.WriteLine(g.verb_stem("likes"));
            Console.Read();
        }
    }
}
