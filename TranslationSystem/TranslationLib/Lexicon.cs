using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace TranslationLib
{
    public class Lexicon
    {
        List<TagWords> Lx = new List<TagWords>();

        List<ValueTag> value_tags;
        List<string>[] language_tags = new List<string>[] { new List<string>() { },
                                                            new List<string>() { } };

        public Lexicon()
        {

        }

        public Lexicon(dbGraph g)
        {
            int i = 0;
            foreach (var t in g.Tables_graph)
            {
                value_tags = g.ReturnDataValues(g.Tables_graph[i].Name);
                i++;
                foreach (var c in g.Tables_graph[i].Columns)
                {
                    i++;
                }
            }
        }

        public void Tagging(string question, Lexicon lx)
        {
            string[] q = question.Split();
            foreach (var w in q)
            {
                string tag = "";
                foreach (var list in language_tags)
                    foreach (var name in list)
                        if (name == w)
                        {
                            lx.Add(new TagWords { Word = w, Category = Tag.Object, Tag = list[0] });
                            continue;
                        }
                foreach (var s in lx.value_tags)
                {
                    if (s.Column == w)
                    {
                        lx.Add(new TagWords { Word = w, Category = Tag.Object, Tag = s.Column });
                        continue;
                    }
                    if (s.Value == w)
                    {
                        lx.Add(new TagWords { Word = w, Category = Tag.Value, Tag = s.Column });
                        continue;
                    }
                }
            }
        }

        public void Add(TagWords word)
        {
            Lx.Add(word);
        }

        public string noun_stem(string s)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.ExecuteFile("../../../TranslationLib/PythonNQS/start.py", scope);
            dynamic function = scope.GetVariable("noun_stem");
            dynamic result = function(s);
            return result.ToString();
        }
    }
}
