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
            Grammar g = new Grammar();

            Console.WriteLine(g.noun_stem("ducks"));
            

            Console.Read();
        }
    }
}
