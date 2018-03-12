using System.Collections.Generic;

namespace TranslationLib
{
    public class Translation
    {
        dbGraph DB;
        Grammar g = new Grammar();
        Lexicon lx;

        private void Tagging_Cat(string question)
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

        private ParseTree Parsing()
        {
            g.POS_Tagging(lx);
            return new ParseTree(new ParseTreeBuilder(lx, g).root);
        }

        private string AbstractSemanticInterpetation(ParseTree tree)
        {
            var topLevelRule = g.TopLevelRule(tree);
            if (topLevelRule == "NP -> Nom" || topLevelRule == "Nom -> AN" || topLevelRule == "AN -> N" || topLevelRule == "N -> 'Np'")
                return AbstractSemanticInterpetation(tree.tree[0]);

            switch (topLevelRule)
            {
                case "S -> WHOSE NP VP":
                    return "(x EXPERT.ID ?v)\n(x " + AbstractSemanticInterpetation(tree.tree[1]) + AbstractSemanticInterpetation(tree.tree[2]) + ")";

                case "S -> WHAT VP OF NP":
                    return "(x " + AbstractSemanticInterpetation(tree.tree[1]) + " ?v)\n(x " + AbstractSemanticInterpetation(tree.tree[3]) + ")";

                case "S -> WHAT VP OF NP OF NP VP":
                    return "(x " + AbstractSemanticInterpetation(tree.tree[1]) + " ?v)\n(x type " + AbstractSemanticInterpetation(tree.tree[3]) + ")\n(x type " + AbstractSemanticInterpetation(tree.tree[5]) + ")\n(x " + AbstractSemanticInterpetation(tree.tree[6]) + ")";

                case "VP -> BE NP":
                    return AbstractSemanticInterpetation(tree.tree[1]);

                case "NP -> P":
                    return AbstractSemanticInterpetation(tree.tree[0]);

                case "VP -> WITH NP LIKE NP":
                    return AbstractSemanticInterpetation(tree.tree[1]) + " " + AbstractSemanticInterpetation(tree.tree[3]);

                case "N -> 'Np'":
                    if (tree.root.word.Category == Tag.Object) return tree.root.word.Tag_TableColumn;
                    else return tree.root.word.Word;

                default: return "";
            }
        }

        private string Query(string AbsSemInterpret)
        {
            List<string> tables = new List<string>();
            foreach (var w in lx.getAll())
            {
                if (w.Category == Tag.Object)
                    tables.Add(w.Tag_TableColumn.Substring(0, w.Tag_TableColumn.IndexOf('.')));
            }

            string join = "";

            for (int i = 0; i < tables.Count; i++)
                for (int j = i; j < tables.Count; j++)
                {
                    string conn = isConnected(tables[i], tables[j]);
                    if (!string.IsNullOrEmpty(conn))
                        join += tables[i] + " join " + tables[j] + conn;
                }

            return "";
        }

        private string isConnected(string t1, string t2)
        {
            foreach (var table1 in DB.Tables_graph)
            {
                if (t1 == table1.Name)
                    foreach (var table2 in DB.Tables_graph)
                        if (t2 == table2.Name)
                            foreach (var col in table2.FK)
                                if (col.Value.Substring(0, col.Value.IndexOf('.')) == t1)
                                    return " on " + col.Value + " == " + t2 + "." + table2.Columns[col.Key];
            }
            return "";
        }
    }
}
