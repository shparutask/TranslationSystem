using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace TranslationLib.Translation
{
    class Parser
    {
        Dictionary<string, Tags> function_words_tags = new Dictionary<string, Tags>() { { "a", Tags.AR }, { "an", Tags.AR }, { "and", Tags.AND },
                                                                                        {"is", Tags.BEs }, { "are", Tags.BEp }, { "does", Tags.DOs },
                                                                                        { "do", Tags.DOp}, {"who", Tags.WHO }, {"which", Tags.WHICH },
                                                                                        { "Who", Tags.WHO }, { "Which",  Tags.WHICH }, { "?", Tags.Q } };

        public Parser(List<TagWords> tagged_words)
        {
            var function_words = function_words_tags.Keys;
        }

        private string verb_stem(string s)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.ExecuteFile("statements.py", scope);
            dynamic function = scope.GetVariable("verb_stem");
            dynamic result = function(s);
            return result;
        }
    }
}
