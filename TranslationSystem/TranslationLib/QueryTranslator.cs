using System.Collections.Generic;

//@"Data Source=SOPHIESHPA\SQLEXPRESS;Initial Catalog=MIGRATION_EXPERT;Integrated Security=True"

namespace TranslationLib
{
    class QueryTranslator
    {
        struct Mark
        {
            public string token;
            public Marks mark;
        }

        enum Marks
        {
            Value,
            Object
        };

        dbGraph m_DB;

        public QueryTranslator(string address)
        {
            m_DB = new dbGraph(address);
        }

        public string FormalQuery(string question)
        {
            //List<Mark> markedTokens = marking(parsing(question));

            return "";
        }

        private List<string> parsing(string question)
        {
            int i = 0;
            List<string> tokens = new List<string>();
            while (i <= question.Length)
            {
                string s = "";
                while (string.Equals(question[i], " "))
                {
                    s += question[i];
                    i++;
                }
                i++;
                tokens.Add(s);
            }
            return tokens;
        }

        private List<Mark> marking(List<string> words)
        {
            List<Mark> markedTokens = new List<Mark>();
            bool wasBroken = false;
            foreach (string s in words)
            {
                wasBroken = false;
                foreach (Table t in m_DB.Tables_graph)
                    if (t.Name == s)
                    {
                        markedTokens.Add(new Mark { token = s, mark = Marks.Object });
                        wasBroken = true;
                        break;
                    }
                if(!wasBroken) markedTokens.Add(new Mark { token = s, mark = Marks.Value });
            }
            return markedTokens;
        }

        private string abstractSemanticInterpretation(List<Mark> markedTokens)
        {
            string interpret = "";
            foreach (Mark s in markedTokens)
            {
                if (s.mark == Marks.Object)
                {
                    interpret += "(x type " + s.token + ")\n";
                }
                else
                {
                    interpret += "(x " + s.token + "?v)\n";
                }
            }
            return interpret += "\b";
        }

        private string concreteSemanticInterpretation(string abst)
        {

            return "";
        }

        private string ranking(string concrete)
        {
            return "";
        }
    }
}
