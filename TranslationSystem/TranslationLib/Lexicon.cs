using System.Collections.Generic;

namespace TranslationLib
{
    public class Lexicon
    {
        public List<ValueTag> value_tags { get; set; }
        public List<string>[] Language_tags {get { return language_tags; } }

        List<string>[] language_tags = new List<string>[] { new List<string>() { },                                                            new List<string>() { } };
        List<TaggedWord> lx = new List<TaggedWord>();

        public Lexicon(dbGraph g)
        {
            int i = 0;
            foreach (var t in g.Tables_graph)
            {
                value_tags = g.ReturnDataValues(g.Tables_graph[i].Name);
                i++;
            }
        }

        public void Add(TaggedWord word)
        {
            lx.Add(word);
        }

        public List<TaggedWord> getAll()
        {
            return lx;
        }

        public void changeLx(List<TaggedWord> new_wlist)
        {
            lx = new_wlist;
        }
    }
}
