using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebConsole
{
    public static class Dataset
    {
        static string filePath = "dataset.tsv";

        public static List<Element> Set = new List<Element>();

        public class Element
        {
            public string Question { get; set; }

            public string SQLQuery { get; set; }

            public double Probability { get; set; }
        }

        public static void GetDataset()
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = reader.ReadLine();
                int count = 0;

                while (line != null)
                {
                    var arr = line.Split('\t');

                    Set.Add(new Element()
                    {
                        Question = arr[0],
                        SQLQuery = arr[1],
                        Probability = Convert.ToDouble(arr[2])
                    });

                    count++;
                    line = reader.ReadLine();
                }

                Set = Set.Select(x => new Element() { Question = x.Question, SQLQuery = x.SQLQuery, Probability = x.Probability / count }).ToList();
            }
        }
    }
}
