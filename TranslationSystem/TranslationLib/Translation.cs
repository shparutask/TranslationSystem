﻿namespace TranslationLib
{
    public class Translation
    {
        private void Tagging_Cat(string question, Lexicon lx)
        {
            string[] q = question.Split();
            bool IsAdded = false;
            foreach (var w in q)
            {
                foreach (var list in lx.Language_tags)
                    foreach (var name in list)
                        if (name == w)
                        {
                            lx.Add(new TaggedWord { Word = w, Category = Tag.Object, Tag_TableColumn = list[0] });
                            IsAdded = true;
                            continue;
                        }
                foreach (var s in lx.value_tags)
                {
                    if (s.Column == w)
                    {
                        lx.Add(new TaggedWord { Word = w, Category = Tag.Object, Tag_TableColumn = s.Column });
                        IsAdded = true;
                        continue;
                    }
                    if (s.Value == w)
                    {
                        lx.Add(new TaggedWord { Word = w, Category = Tag.Value, Tag_TableColumn = s.Column });
                        IsAdded = true;
                        continue;
                    }
                }
                if (!IsAdded)
                {
                    lx.Add(new TaggedWord { Word = w });
                }
                IsAdded = false;
            }
        }

        private ParseTree Parsing(Lexicon lx)
        {
            Grammar g = new Grammar();
            g.POS_Tagging(lx);
            return new ParseTree(lx, g);
        }

        private void AbstractSemanticInterpetation(ParseTree tree)
        {

        }
    }
}
