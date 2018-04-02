using System.Collections.Generic;

namespace TranslationLib
{
    public class Translation
    {
        dbGraph DB = new dbGraph(@"Data Source = SOPHIESHPA\SQLEXPRESS; Initial Catalog = MIGRATION_EXPERT; Integrated Security = True");
        Grammar g = new Grammar();
        Lexicon lx;

        public Translation()
        {
            lx = new Lexicon(DB);
        }

        public string ToQuery(string question)
        {
            Tagging_Cat(question);
            return Query(AbstractSemanticInterpetation(Parsing()));
        }

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
            if (tree.root.word != null)
                if (tree.root.word.Category == Tag.Object) return tree.root.word.Tag_TableColumn;
                else return tree.root.word.Word + " " + tree.root.word.Tag_TableColumn;

            var topLevelRule = g.TopLevelRule(tree);
            if (topLevelRule == "NP -> Nom" || topLevelRule == "Nom -> AN" || topLevelRule == "AN -> N" || topLevelRule == "N -> 'Np'")
                return AbstractSemanticInterpetation(tree.tree[0]);

            switch (topLevelRule)
            {
                case "S -> WHOSE NP VP":
                    return "EXPERT.ID ?v\n" + AbstractSemanticInterpetation(tree.tree[1]) + AbstractSemanticInterpetation(tree.tree[2]);

                case "S -> WHAT VP OF NP":
                    return AbstractSemanticInterpetation(tree.tree[1]) + " ?v\n" + AbstractSemanticInterpetation(tree.tree[3]);

                case "S -> WHAT VP OF NP OF NP VP":
                    return AbstractSemanticInterpetation(tree.tree[1]) + " ?v\ntype " + AbstractSemanticInterpetation(tree.tree[3]) + "\n" + AbstractSemanticInterpetation(tree.tree[6]) + " like " + AbstractSemanticInterpetation(tree.tree[5]);

                case "VP -> BE NP":
                    return AbstractSemanticInterpetation(tree.tree[1]);

                case "NP -> P":
                    return AbstractSemanticInterpetation(tree.tree[0]);

                case "NP -> AR Nom":
                    return AbstractSemanticInterpetation(tree.tree[1]);

                case "Nom -> AN":
                    return AbstractSemanticInterpetation(tree.tree[0]);

                case "AN -> N":
                    return AbstractSemanticInterpetation(tree.tree[0]);

                case "VP -> LIKE NP":
                    return AbstractSemanticInterpetation(tree.tree[1]);

                default: return "";
            }
        }

        private string Query(string AbsSemInterpret)
        {
            string select = "";
            var sems = AbsSemInterpret.Split('\n');
            List<string> tables = new List<string>();
            List<string> wheres = new List<string>();

            foreach (var s in sems)
            {
                var words = s.Split(' ');
                if (words[0] == "type")
                    tables.Add(words[1]);
                else
                    if (words[1] == "?v")
                {
                    var w = words[0].Split('.');
                    select = w[1];
                    tables.Add(w[0]);
                }
                else if (words.Length > 1)
                {
                    string value = "";
                    int indexLike = -1;
                    for (int i = 0; i < words.Length - 1; i++)
                    {
                        if (words[i] == "like")
                            indexLike = i;
                        else
                            value += " " + words[i];
                    }
                    if (indexLike == -1) wheres.Add(words[words.Length - 1] + " = '" + value + "'");
                    else wheres.Add(words[words.Length - 1] + " like '" + value + "'");
                    tables.Add(words[words.Length - 1].Substring(0, words[words.Length - 1].IndexOf('.')));
                }
            }

            string join = tables.Count > 1 ? "" : tables[0];

            for (int i = 0; i < tables.Count; i++)
                for (int j = i + 1; j < tables.Count; j++)
                {
                    Table tab1 = null, tab2 = null;
                    foreach (var table in DB.Tables_graph)
                    {
                        if (tables[i] == table.Name)
                            tab1 = table;
                        if (tables[j] == table.Name)
                            tab2 = table;
                    }
                    string conn = Join(tab1, tab2);
                    if(string.IsNullOrEmpty(conn)) conn = Join(tab2, tab1);
                    if (!string.IsNullOrEmpty(conn))
                        join += conn;
                }

            return "select " + select + " from " + join + " where " + wheres[0];
        }

        private string Join(Table t1, Table t2)
        {
            if (t1 == t2)
                return t2.Name;
            else
            {
                foreach (var f in t1.FK)
                {
                    string j = Join(f.Value.t, t2);
                    if (!string.IsNullOrEmpty(j))
                        return j + " join " + t1.Name + " on " + f.Value.t.Name + "." + f.Value.column + " = " + t1.Name + "." + f.Key.ToString();
                }
            }
            return "";
        }
    }
}
