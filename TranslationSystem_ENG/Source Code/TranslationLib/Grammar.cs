﻿using System.Collections.Generic;

namespace TranslationLib
{
    public class Grammar
    {
        public List<Rule> Rules;

        public Grammar()
        {
            Rules = new List<Rule>();
            foreach (var s in rules)
            {
                Rules.Add(new Rule(s));
            }
        }

        public string TopLevelRule(ParseTree tree)
        {
            var rights = "";
            foreach (var c in tree.root.children)
            {
                rights = rights + c.value + " ";
            }
            return tree.root.value + " -> " + rights.Substring(0, rights.Length - 1);
        }

        public string isFunction(string w)
        {
            for (int i = 0; i < function_words_tags.GetLength(0); i++)
                for (int j = 0; j < function_words_tags.GetLength(1); j++)
                {
                    if (w == function_words_tags[i, j]) return function_words_tags[i, 0];
                }
            return "";
        }       

        public void POS_Tagging(Lexicon lx)
        {
            /* Grammar for the statement language is:
            #   S  -> P is AR Ns | P is A | P Is | P Ts P
            #   AR -> a | an*/
            var wlist = lx.getAll();
            int indexLike = -1;

            foreach (var w in wlist)
            {
                if ('A' <= w.Word[0] && w.Word[0] <= 'Z' && isFunction(w.Word) == "")
                    w.POSTag = "P";

                string f = isFunction(w.Word);
                if (!string.IsNullOrEmpty(f))
                {
                    w.POSTag = f;
                    if (f == "LIKE")
                    {
                        indexLike = wlist.IndexOf(w);
                    }
                }
            }

            if (indexLike != -1)
            {
                for (int i = indexLike + 2; i < wlist.Count; i++)
                {
                    wlist[indexLike + 1].Word += " " + wlist[i].Word;
                }

                wlist.RemoveRange(indexLike + 2, wlist.Count - indexLike - 2);
                wlist[indexLike + 1].POSTag = "NP";
            }

            if (wlist[1].Word == "is")
            {
                if (wlist[2].Word == "a" || wlist[2].Word == "an" || wlist[2].Word == "the")
                    if (wlist[3].POSTag != "N") wlist[3].POSTag = "N";
                    else
                         if (wlist[2].POSTag != "A") wlist[3].POSTag = "A";
            }

            if (wlist[0].Word == "Whose")
                wlist[1].POSTag = "NP";

            for (int i = 0; i < wlist.Count; i++)
            {
                if (wlist[i].Word == "of")
                    wlist[i + 1].POSTag = "NP";

                if (wlist[i].Word == "a" || wlist[i].Word == "an" || wlist[i].Word == "the" || wlist[i].Word == "with")
                    if (wlist[i + 1].POSTag != "N") wlist[i + 1].POSTag = "N";

                if (i > 1 && (wlist[i].Word == "is") && wlist[i - 1].POSTag == "NP" && string.IsNullOrEmpty(wlist[i + 1].POSTag))
                {
                    wlist[i + 1].POSTag = "NP";
                    for (int j = i + 2; j < wlist.Count; j++)
                        wlist[i + 1].Word += " " + wlist[j].Word;
                    wlist.RemoveRange(i + 2, wlist.Count - i - 2);
                }

                if (string.IsNullOrEmpty(wlist[i].POSTag))
                {
                    if (string.IsNullOrEmpty(wlist[i].POSTag))
                        wlist[i].POSTag = "NP";
                }
            }

            lx.changeLx(wlist);
        }

        string[] rules = new string[] {
            "VP -> T NP|BE A|VP AND VP|LIKE NP|BE NP|NP BE I",
            "NP -> P|AR Nom|Nom|ANSO NP",
            "Nom -> AN|AN Rel",
            "AN -> N|A AN",
            "Rel -> WHO VP|NP T",
            "WHAT -> BE THERE",
            "OF -> WHO HAVE",
            "WITH -> WHICH HAVE",
            "WHOSE -> WHOs HAVE|WHOSE I",
            "S -> WHAT VP OF NP WITH NP VP|WHAT VP OF NP OF NP WITH NP VP|WHAT VP OF NP OF NP VP|WHAT VP OF NP|WHAT QP OF NP|WHAT VP OF NP VP|WHOSE NP VP|WHAT VP HERE|WHAT VP|WHOSE NP BE VP|WHOSE NP WITH NP VP|WHAT NP VP"
        };

        string[,] function_words_tags = new string[,] {
           {"BE", "is", "Are", "are" },
           {"AR", "a", "an", "the" },
           {"ANSO", "any", "some", "Any"  },
           {"AND", "and", "", ""  },
           {"WHOs", "Who", "", "" },
           {"WHO", "who", "", "" },
           {"WHICH", "which", "" , ""},
           {"WHAT" , "What" , "Which" , ""},
           {"OR", "or" , "", ""},
           {"OF", "of", "by", ""},
           {"LIKE", "like", "about", "in"},
           {"WITH", "with", "", ""},
           {"THERE", "There", "there", ""},
           {"WHOSE", "Whose", "", ""},
           {"HERE", "Here", "here", ""},
           {"HAVE", "has", "have", ""},
           {"I", "written", "", ""}
           };
    }
}
