namespace TranslationLib
{
    class ParseTree
    {
        ParseNode root;

        public ParseTree(Lexicon lx)
        {
            var tagged_words = lx.getAll();
            ParseNode[] leaves = new ParseNode[tagged_words.Count];
            int i = 0;
            foreach (var w in tagged_words)
            {
                foreach (var t in w.POSTags)
                    leaves[i] = new ParseNode { value = t, children = null };
                i++;
            }


        }
    }
}
