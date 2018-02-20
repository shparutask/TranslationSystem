using System.Collections.Generic;

namespace TranslationLib
{
    class Lexicon
    {
        List<ValueTag> value_tags;
        List<string> language_tags;

        public List<ValueTag> Value_tags { get { return value_tags; } }
        public List<string> Language_tags { get { return language_tags; } }

        public Lexicon(dbGraph g)
        {
            language_tags = new List<string>();
            int i = 0;
            foreach (var t in g.Tables_graph)
            {
                language_tags.Add(g.Tables_graph[i].Name);
                value_tags = g.ReturnDataValues(g.Tables_graph[i].Name);
                i++;
                foreach (var c in g.Tables_graph[i].Columns)
                {
                    language_tags[i] = c;
                    i++;
                }
            }
        }
    }
}
