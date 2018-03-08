using System.Collections.Generic;

namespace TranslationLib
{
    class ParseTree
    {
        ParseNode root = new ParseNode { value = "S", children = null };
        List<ParseNode> top_level;
        Grammar g;

        public ParseTree(Lexicon lx, Grammar g)
        {
            this.g = g;
            var tagged_words = lx.getAll();
            top_level = new List<ParseNode>();
            int i = 0;
            foreach (var w in tagged_words)
            {
                foreach (var t in w.POSTags)
                    top_level.Add(new ParseNode { value = t, children = null });
                i++;
            }

            var nodes = new List<ParseNode>();

            while (top_level.Count > 1)
            {
                foreach (var t in top_level)
                {
                    string s = containsRight(t.value);
                    if (s != "")
                    {
                        nodes.Add(new ParseNode { value = s, children = t });
                    }
                }
                top_level = nodes;
            }
        }

        private string containsRight(string token)
        {
            foreach (var r in g.Rules)
            {
                foreach (var t in r.right)
                    if (t == token)
                        return t;
            }
            return "";
        }
    }
}
