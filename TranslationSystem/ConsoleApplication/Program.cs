using System;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;

namespace ConsoleApplication
{
    class Program
    {       
        static void Main(string[] args)
        {/*
             // вызываем функцию и получаем результат
          //   dynamic result = function(x);

              string conn = @"Data Source = SOPHIESHPA\SQLEXPRESS;Initial Catalog=MIGRATION_CENTER;Integrated Security=True";
              dbGraph DB = new dbGraph(conn);
              var values = DB.ReturnDataValues("Country");
              foreach(var s in values)
              {
                  Console.WriteLine(s.column + " " + s.value);
              }
              Console.ReadKey();*/

             ScriptEngine engine = Python.CreateEngine();
             ScriptScope scope = engine.CreateScope();
             var paths = engine.GetSearchPaths();
             paths.Add(@"C:\Users\Sonya\Desktop\nltk-3.2.5");
             paths.Add(@"C:\Users\Sonya\Desktop");
             paths.Add(@"C:\python27\Lib");
             paths.Add(@"C:\Users\Sonya\Desktop\ВКР\TranslationSystem - git\TranslationSystem\ConsoleApplication\Lib");

             paths.Add(@"C:\Users\Sonya\Desktop\ВКР\TranslationSystem - git\TranslationSystem\ConsoleApplication");
             engine.SetSearchPaths(paths);
             var source = engine.CreateScriptSourceFromFile(@"C:\Users\Sonya\Desktop\ВКР\TranslationSystem - git\TranslationSystem\TranslationLib\PythonNQS\start.py");
             source.Execute(scope);


             //engine.ExecuteFile(@"C:\Users\Sonya\Desktop\ВКР\TranslationSystem - git\TranslationSystem\TranslationLib\PythonNQS\semantics.py");

             //dynamic function = engine.;*/
            Console.ReadKey();
         }
    }
}
