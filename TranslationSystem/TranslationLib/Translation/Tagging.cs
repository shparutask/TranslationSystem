using System.Collections.Generic;

namespace TranslationLib.Translation
{
    class Tagging
    {
        public List<TagWords> Tagged_words { get { return tagged_words; } }
        List<TagWords> tagged_words = new  List<TagWords>();

        public Tagging(string question, Lexicon lx)
        {
            string[] q = question.Split();
            foreach (var w in q)
            {
                Tags t = Tags.Another;
                if (lx.Language_tags.Contains(w)) t = Tags.Table;
                else
                    foreach (var s in lx.Value_tags)
                    {
                        if (s.column == w) t = Tags.Column;
                    }
                tagged_words.Add(new TagWords { word = w, tag = t });
            }
        }
    }
}
