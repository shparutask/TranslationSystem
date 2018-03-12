using System.Collections.Generic;

namespace TranslationLib
{
    public class ParseTreeBuilder
    {
        public ParseNode root = new ParseNode {value = "S", children = null };
        List<ParseNode> top_level;
        public List<List<ParseNode>> tree = new List<List<ParseNode>>();
        Lexicon lx;

        Grammar g;

        public ParseTreeBuilder(Lexicon lx, Grammar g)
        {
            this.lx = lx;
            this.g = g;
            var tagged_words = lx.getAll();
            top_level = new List<ParseNode>();
            int i = 0;
            foreach (var w in tagged_words)
            {
                foreach (var t in w.POSTags)
                    top_level.Add(new ParseNode { value = t, children = null, word = w });
                i++;
            }
            tree[0] = top_level;

            var nodes = new List<ParseNode>();

            while (top_level.Count > 1)
            {
                foreach (var t in top_level)
                {
                    string s = containsInRight(t.value);
                    if (s != "")
                    {
                        foreach (var n in nodes)
                        {
                            if (n.value == s)
                                n.children.Add(t);
                        }
                        nodes.Add(new ParseNode { value = s, children = new List<ParseNode> { t } });
                    }
                }
                tree.Add(nodes);
            }

            root = top_level[0];
        }

        private string containsInRight(string token)
        {
            foreach (var r in g.Rules)
            {
                foreach (var t in r.right)
                    if (t == token)
                        return r.left;
            }
            return "";
        }
        
    }
}
