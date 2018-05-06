using System.Collections.Generic;

namespace TranslationLib
{
    public class ParseTreeBuilder
    {
        public ParseNode root = new ParseNode { value = "S", children = null };
        List<ParseNode> top_level;
        public List<List<ParseNode>> tree = new List<List<ParseNode>>();

        public ParseTreeBuilder(Lexicon lx, Grammar g)
        {
            var tagged_words = lx.getAll();
            top_level = new List<ParseNode>();

            foreach (var w in tagged_words)
            {
                top_level.Add(new ParseNode { value = w.POSTag, children = null, word = w });
            }

            tree.Add(top_level);

            while (tree[tree.Count - 1].Count > 1)
            {
                if (tree.Count > 50)
                {
                    root = null;
                    return;
                }
                top_level = new List<ParseNode>();
                int count = tree[tree.Count - 1].Count;
                var child = new List<ParseNode>();

                var rule = containsInRight(tree[tree.Count - 1], g);

                top_level = new List<ParseNode>();

                foreach (var n in tree[tree.Count - 1])
                {
                    bool isAdded = false;
                    foreach (var r in rule)
                    {
                        var left = leftNode(top_level, r.left);
                        var node = rightNodes(tree[tree.Count - 1], r.right);
                        if (r.left == "S" && node.Count != tree[tree.Count - 1].Count) continue;

                        if (node.Contains(n))
                        {
                            if (left == null || left.word != null)
                                top_level.Add(new ParseNode { value = r.left, children = node });
                            else
                            {
                                if (left.children == null)
                                    left.children = new List<ParseNode>();
                                if (!left.children.Contains(n))
                                    if (leftContains(left.children, n))// && node.Contains(n))
                                        left.children.Add(n);
                                    else
                                        top_level.Add(new ParseNode { value = r.left, children = node });

                            }
                            isAdded = true;
                        }
                    }
                    if (!isAdded && !top_level.Contains(n))
                        top_level.Add(n);
                }

                tree.Add(top_level);
            }

            root = tree[tree.Count - 1][0];
        }

        private List<Rule> containsInRight(List<ParseNode> tokens, Grammar g)
        {
            List<Rule> rules = new List<Rule>();

            int length = tokens.Count;
            int count = length;
            while (count > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    string s = "";
                    for (int j = i; j <= length - count; j++)
                        s += tokens[j].value + " ";

                    if (!string.IsNullOrEmpty(s))
                    {
                        s = s.Substring(0, s.Length - 1);
                        foreach (var r in g.Rules)
                        {
                            foreach (var t in r.right)
                            {
                                if (s == t)
                                    rules.Add(new Rule { left = r.left, right = new List<string> { t } });
                            }
                        }
                    }
                }
                count--;
            }
            return rules;
        }

        private ParseNode leftNode(List<ParseNode> top_level, string value)
        {
            for (int i = top_level.Count - 1; i >= 0; i--)
            {
                if (top_level[i].value == value)
                    return top_level[i];
            }
            return null;
        }

        private List<ParseNode> rightNodes(List<ParseNode> top_level, List<string> right)
        {
            List<ParseNode> rights = new List<ParseNode>();

            string[] tokens = right[0].Split(' ');
            int i = 0;
            foreach (var t in top_level)
            {
                if (tokens[i] == t.value)
                {
                    rights.Add(t);
                    i++;
                }

                if (i >= tokens.Length) break;
            }

            return rights;
        }

        private bool leftContains(List<ParseNode> top_level, ParseNode value)
        {
            foreach (var n in top_level)
                if (value == n) return true;
            return false;
        }
    }
}
