using TranslationLib;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            dbGraph DB = new dbGraph(@"Data Source=SOPHIESHPA\SQLEXPRESS;Initial Catalog=MIGRATION_CENTER;Integrated Security=True");
        }
    }
}
