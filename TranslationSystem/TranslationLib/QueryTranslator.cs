using System.Collections.Generic;
//@"Data Source=SOPHIESHPA\SQLEXPRESS;Initial Catalog=MIGRATION_EXPERT;Integrated Security=True"

namespace TranslationLib
{
    class QueryTranslator
    {
        enum Mark
        {
            Value,
            Obj,
            Prop
        }

        dbGraph m_DB;

        public QueryTranslator(string address)
        {
            m_DB = new dbGraph(address);
        }

        private string[] getTokens(string question)
        {
            return question.Split(' ');
        }

        private Dictionary<Mark, string> tagging(string[] tokens)
        {
            Dictionary<Mark, string> tags = new Dictionary<Mark, string>();
            foreach (string t in tokens)
            {
                bool IsMarked = false;
                if (tablesContains(t) && !IsMarked) { tags.Add(Mark.Obj, t); IsMarked = true; }
                if (attrContains(t) && !IsMarked) { tags.Add(Mark.Value, t); IsMarked = true; }
                if (!IsMarked) tags.Add(Mark.Value, t);
            }
            return tags;
        }

        private bool tablesContains(string table)
        {
            foreach (Table t in m_DB.Tables_graph)
            {
                if (t.Name == table) return true;
            }
            return false;
        }

        private bool attrContains(string property)
        {
            foreach (Table t in m_DB.Tables_graph)
            {
                foreach (string col in t.Columns)
                    if (col == property) return true;
            }
            return false;
        }
    }
}
