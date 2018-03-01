using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using TranslationLib;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(new Lexicon().noun_stem("References"));
            Console.ReadKey();
        }
    }
}
