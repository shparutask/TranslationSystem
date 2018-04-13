using System.Collections.Generic;
using System;

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
            lx.getAll().Clear();
            int questmark = question.IndexOf('?');

            string[] q = questmark == -1 ? question.Split() : question.Substring(0, questmark).Split();
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
                    if (!IsAdded && string.IsNullOrEmpty(g.isFunction(w)) && (s.Value == w || s.Value.Contains(w)))
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
                else
                    if (!string.IsNullOrEmpty(tree.root.word.Tag_TableColumn))
                    return tree.root.word.Word + ";" + tree.root.word.Tag_TableColumn;
                else
                    return tree.root.word.Word;

            var topLevelRule = g.TopLevelRule(tree);
            if (topLevelRule == "NP -> Nom" || topLevelRule == "Nom -> AN" || topLevelRule == "AN -> N" || topLevelRule == "N -> 'Np'")
                return AbstractSemanticInterpetation(tree.tree[0]);

            switch (topLevelRule)
            {
                case "S -> WHOSE NP VP":
                    if (AbstractSemanticInterpetation(tree.tree[2]).Contains(";")) return "FIRST_NAME.NAME_ENG ?v\n" + "MIDDLE_NAME.NAME_ENG ?v\n" + "LAST_NAME.NAME_ENG ?v\n" + AbstractSemanticInterpetation(tree.tree[2]).Split(';')[0] + " like " + AbstractSemanticInterpetation(tree.tree[2]).Split(';')[1];
                    else return "FIRST_NAME.NAME_ENG ?v\n" + "MIDDLE_NAME.NAME_ENG ?v\n" + "LAST_NAME.NAME_ENG ?v\n" + AbstractSemanticInterpetation(tree.tree[2]) + " like " + AbstractSemanticInterpetation(tree.tree[1]);

                case "S -> WHAT VP OF NP":
                    if (AbstractSemanticInterpetation(tree.tree[3]).Contains(";")) return AbstractSemanticInterpetation(tree.tree[1]) + " ?v\n" + AbstractSemanticInterpetation(tree.tree[3]).Split(';')[0] + " like " + AbstractSemanticInterpetation(tree.tree[3]).Split(';')[1];
                    return AbstractSemanticInterpetation(tree.tree[1]) + " ?v\n" + AbstractSemanticInterpetation(tree.tree[3]);

                case "S -> WHAT VP THERE":
                    return AbstractSemanticInterpetation(tree.tree[1]) + " ?v";

                case "S -> WHAT VP OF NP OF NP VP":
                    return AbstractSemanticInterpetation(tree.tree[1]) + " ?v\ntype " + AbstractSemanticInterpetation(tree.tree[3]) + "\n" + AbstractSemanticInterpetation(tree.tree[6]).Split(';')[0] + " like " + AbstractSemanticInterpetation(tree.tree[6]).Split(';')[1];

                case "S -> WHAT VP OF NP OF NP WITH NP VP":
                    return AbstractSemanticInterpetation(tree.tree[1]) + " ?v\ntype " + AbstractSemanticInterpetation(tree.tree[3]) + "\n" + AbstractSemanticInterpetation(tree.tree[8]).Split(';')[0] + " like " + AbstractSemanticInterpetation(tree.tree[8]).Split(';')[1];

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

                if (words[0] == "EXPERT")
                {
                    select = sems.Length == 1 ? "DEGREE  " : "EXPERT.*";
                    tables.Add(words[0]);
                }

                if (words[0] == "type")
                    if (!tables.Contains(words[1])) tables.Add(words[1]); else { }
                else if (words[1] == "?v")
                {
                    var w = words[0].Split('.');
                    if (w.Length > 1)
                    {
                        if (!tables.Contains(w[0])) tables.Add(w[0]);
                    }
                    select += words[0] + ", ";
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
                    if (indexLike == -1 && Int32.TryParse(value.Substring(1, value.Length - 1), out indexLike)) wheres.Add(words[words.Length - 1] + " = " + indexLike.ToString());
                    else wheres.Add(words[words.Length - 1] + " like '%" + value.Substring(1, value.Length - 1) + "%'");
                    if (!tables.Contains(words[words.Length - 1].Substring(0, words[words.Length - 1].IndexOf('.')))) tables.Add(words[words.Length - 1].Substring(0, words[words.Length - 1].IndexOf('.')));
                }
            }

            List<string> join = new List<string>();

            for (int i = 0; i < tables.Count - 1; i++)
            {
                Table tab1 = null, tab2 = null;
                foreach (var table in DB.Tables_graph)
                {
                    if (tables[i] == table.Name)
                        tab1 = table;
                    if (tables[i + 1] == table.Name)
                        tab2 = table;
                }

                string conn = Join(tab1, tab2);
                if (string.IsNullOrEmpty(conn)) conn = Join(tab2, tab1);
                if (string.IsNullOrEmpty(conn)) conn = ComplexJoin(tab1, tab2);
                if (!string.IsNullOrEmpty(conn))
                    join.Add(conn);
            }

            string JoinFrom = "";

            if(tables.Count == 0)
            {
                tables.Add(select.Substring(0, select.Length - 2));
                select = "TITLE  ";
            }

            if (join.Count == 0) JoinFrom = tables[0];
            else
            {
                if (join.Count >= 1)
                {
                    JoinFrom = join[0];

                    for (int i = 1; i < join.Count; i++)
                    {
                        string ex = IndexSubstring(join[i], join[i - 1]);
                        if (string.IsNullOrEmpty(ex)) JoinFrom += " " + join[i].Substring(join[i].IndexOf("join"), join[i].Length - join[i].IndexOf("join"));
                        else JoinFrom += " " + join[i].Substring(join[i].IndexOf(ex) + ex.Length, join[i].Length - join[i].IndexOf(ex) - ex.Length);
                    }
                }
            }

            string where = "";
            if (wheres.Count > 0) where = " where " + wheres[0];
            return "select " + select.Substring(0, select.Length - 2) + " from " + JoinFrom + where;
        }

        private string ComplexJoin(Table t1, Table t2)
        {
            bool isFinded = false;
            Table Ft = null;
            string col1 = "", col2 = "", fcol1 = "", fcol2 = "";
            foreach (var t in DB.Tables_graph)
            {
                int d = 0;
                foreach (var f in t.FK)
                {
                    if (!isFinded)
                    {
                        if (f.Value.t == t1)
                        {
                            col1 = f.Value.column;
                            fcol1 = f.Key;
                            d++;
                        }
                        if (f.Value.t == t2)
                        {
                            col2 = f.Value.column;
                            fcol2 = f.Key;
                            d++;
                        }

                        if (d == 2)
                        {
                            Ft = t;
                            isFinded = true;
                        }
                    }
                }
            }
            if (isFinded) return t1.Name + " join " + Ft.Name + " on " + t1.Name + "." + col1 + " = " + Ft.Name + "." + fcol1 + " join " + t2.Name + " on " + t2.Name + "." + col2 + " = " + Ft.Name + "." + fcol2;
            return "";
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

        private string IndexSubstring(string join1, string join2)
        {
            string[] J1 = join1.Split();
            string[] J2 = join2.Split();
            for (int i = 1; i < J1.Length; i++)
                for (int j = 1; j < J2.Length; j++)
                {
                    if (J1[i] == J2[j] && J1[i - 1] == J2[j - 1] && J1[i - 1] == "join") return J1[i - 1] + " " + J1[i] + " on " + J1[i + 2] + " = " + J1[i + 4];
                }

            return "";
        }
    }
}
