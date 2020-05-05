using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> strings = new List<string>();

            // создаем каталог для файла
            string path = @"C:\Users\sofis\Desktop\12 семестр\ВКР\TranslationSystem\TranslateSQL_RUS\Source Code\Probabilities\data";
            string textFromFile = string.Empty;

            // чтение из файла
            using (StreamReader fstream = new StreamReader($"{path}\\dataset.tsv", System.Text.Encoding.Default))
            {
                while (textFromFile != null)
                {
                    textFromFile = fstream.ReadLine();
                    strings.Add(textFromFile);
                }
            }

            // запись в файл
            using (StreamWriter outstream = new StreamWriter($"{path}\\dataset.tsv"))
            {
                Random rnd = new Random();
                foreach (string str in strings)
                {
                    if (str != null)
                        outstream.WriteLine($"{str.Split('\t')[0]}\t{str.Split('\t')[1]}\t{rnd.Next(1, strings.Count)}");
                }
            }
            Console.ReadLine();
        }

    }
}
