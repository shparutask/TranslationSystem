﻿using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

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

        private string noun_stem(string x)
        {
            var fileName = "C:/Users/Sonya/Desktop/VCR/TranslationSystem_git/TranslationSystem/TranslationLib/noun_stem.py";
            ProcessStartInfo start = new ProcessStartInfo(@"C:\Users\Sonya\AppData\Local\Programs\Python\Python36\python.exe", fileName);
            start.Arguments = string.Format(fileName + " " + x);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    process.WaitForExit();
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script                   
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")

                    if (string.IsNullOrEmpty(result)) return "";
                    return result.Substring(0, result.Length - 2);
                }
            }
            /*ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            engine.ExecuteFile("../../../TranslationLib/noun_stem.py", scope);
            dynamic function = scope.GetVariable("noun_stem");
            dynamic result = function(x);
            return result.ToString();*/
        }

        public string verb_stem(string x)
        {
            var fileName = "C:/Users/Sonya/Desktop/VCR/TranslationSystem_git/TranslationSystem/TranslationLib/verb_stem.py";
            ProcessStartInfo start = new ProcessStartInfo(@"C:\Users\Sonya\AppData\Local\Programs\Python\Python36\python.exe", fileName);
            start.Arguments = string.Format(fileName + " " + x);
            start.UseShellExecute = false;// Do not use OS shell
            start.CreateNoWindow = true; // We don't need new window
            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    process.WaitForExit();
                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script                   
                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")

                    return result.Substring(0, result.Length - 2);
                }
            }
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

                string stem = noun_stem(w.Word);
                if (string.IsNullOrEmpty(stem))
                    w.POSTag = "N";

                string f = isFunction(w.Word);
                if (!string.IsNullOrEmpty(f))
                {
                    w.POSTag = f;
                    if (f == "LIKE" || f == "ABOUT")
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
            else
            {
                string stem = verb_stem(wlist[1].Word);
                if (wlist.Count == 2)
                {
                    wlist[1].Word = stem;
                    if (wlist[1].POSTag != "I") wlist[1].POSTag = "I";
                }
                else
                if (wlist[2].POSTag == "P")
                {
                    wlist[1].Word = stem;
                    if (wlist[1].POSTag != "T") wlist[1].POSTag = "T";
                }
            }

            if (wlist[0].Word == "Whose")
                wlist[1].POSTag = "NP";

            for (int i = 0; i < wlist.Count - 1; i++)
            {
                if (wlist[i].Word == "of")
                    wlist[i + 1].POSTag = "NP";

                if (wlist[i].Word == "a" || wlist[i].Word == "an" || wlist[i].Word == "the" || wlist[i].Word == "with")
                    if (wlist[i + 1].POSTag != "N") wlist[i + 1].POSTag = "N";

                if (i > 1 && wlist[i].Word == "is" && wlist[i - 1].POSTag == "NP")
                {
                    wlist[i + 1].POSTag = "NP";
                    for (int j = i + 2; j < wlist.Count; j++)
                        wlist[i + 1].Word += " " + wlist[j].Word;
                    wlist.RemoveRange(i + 2, wlist.Count - i - 2);
                }
            }

            lx.changeLx(wlist);
        }

        string[] rules = new string[] {
            "VP -> I|T NP|BE A|VP AND VP|LIKE NP|BE NP",            
            "NP -> P|AR Nom|Nom",
            "Nom -> AN|AN Rel",
            "AN -> N|A AN",
            "Rel -> WHO VP|NP T",
            "BE -> BEs|BEp",
            "OF -> WHO HAVE",
            "WITH -> WHICH HAVE",
            "WHOSE -> WHOs HAVE",
            "S -> WHAT VP OF NP OF NP WITH NP VP|WHAT VP OF NP OF NP VP|WHAT VP OF NP|WHAT VP OF NP VP|WHOSE NP VP|WHAT VP THERE"
        };

        string[,] function_words_tags = new string[,] {
           {"BEs", "is", "was", "" },
           {"BEp", "are", "were", "" },
           {"AR", "a", "an", "the" },
           {"AND", "and", "", ""  },
           {"WHOs", "Who", "", "" },
           {"WHO", "who", "", "" },
           {"WHICH", "Which" , "which", ""},
           {"WHAT" , "What" , "", ""},
           {"OR", "or" , "", ""},
           {"OF", "of", "", ""},
           {"LIKE", "like", "about", ""},
           {"WITH", "with", "", ""},
           {"WHOSE", "Whose", "", ""},
           {"THERE", "There", "there", ""},
           {"HAVE", "has", "have", ""}
           };
    }
}
