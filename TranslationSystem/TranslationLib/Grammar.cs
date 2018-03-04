using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;

namespace TranslationLib
{
    public class Grammar
    {
        public string[] Rules { get { return rules; } }

        string[] rules = new string[] {
            "S  -> WHAT QP OF NP WITH NP QP  | WHAT QP OF NP | WHAT QP OF NP ABOUT NP | WHOSE NP QP | WHOSE NP | WHOSE NP OF NP",
            "QP -> VP",
            "VP -> I | T NP | BE A | BE NP | VP AND VP | LIKE NP",
            "NP  -> P | AR Nom | Nom",
            "Nom -> AN | AN Rel",
            "AN  -> N | A AN",
            "Rel  -> WHO VP | NP T",
            "N  -> 'Ns' | 'Np'",
            "I  -> 'Is' | 'Ip'",
            "T  -> 'Ts' | 'Tp'",
            "A  -> 'A'",
            "P  -> 'P'",
            "BE  -> 'BEs' | 'BEp'",
            "AR -> 'AR'",
            "WHO -> 'WHO'",
            "WHAT -> 'WHICH' | 'WHAT'",
            "AND   -> 'AND'",
            "OR -> 'OR'",
            "OF -> 'OF'",
            "LIKE -> 'LIKE'",
            "WITH -> 'WITH'",
            "ABOUT -> 'ABOUT'",
            "WHOSE -> 'WHOSE' | 'WHO HAS'" };

        public string noun_stem(string x)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            engine.ExecuteFile("../../../TranslationLib/noun_stem.py", scope);
            dynamic function = scope.GetVariable("noun_stem");
            dynamic result = function(x);
            return result.ToString();
        }

        string[][] function_words_tags = new string[][] {
           new string[] {"BEs", "is", "was" },
           new string[] { "BEp", "are", "were" },
           new string[] {"AR", "a", "an" },
           new string[] {"AND", "and"  },
           new string[] {"WHO", "Who" },
           new string[] {"WHICH", "Which" },
           new string[] {"WHAT" , "What" },
           new string[] {"OR", "or" },
           new string[] {"OF", "of"},
           new string[] {"LIKE", "like"},
           new string[] {"WITH", "with"},
           new string[] {"ABOUT", "about"},
           new string[] { "WHOSE", "Whose" }
};

        //string[] function_words = function_words_tags[0][];
    }
}
