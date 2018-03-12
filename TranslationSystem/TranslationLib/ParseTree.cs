using System.Collections.Generic;

namespace TranslationLib
{
    public class ParseTree
    {
        public List<ParseTree> tree = new List<ParseTree>();
        public ParseNode root;

        public ParseTree(ParseNode root)
        {
            this.root = root;
            if (root.children != null)
                foreach (var s in root.children)
                    tree.Add(new ParseTree(new ParseNode { value = s.value, children = s.children }));
        }
    }
}
