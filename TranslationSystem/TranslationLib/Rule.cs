using System.Collections.Generic;

namespace TranslationLib
{
    public class Rule
    {
        public string left;
        public List<string> right = new List<string>();

        public Rule(string rule)
        {
            left = rule.Substring(0, rule.IndexOf(' '));
            string r = rule.Substring(rule.IndexOf('>') + 1, rule.Length - 1);
            string[] rs = r.Split('|');

            foreach(var s in rs)
            {
                right.Add(s);
            }
        }
    }
}
