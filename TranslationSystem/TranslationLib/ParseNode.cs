using System.Collections.Generic;

namespace TranslationLib
{
   public class ParseNode
    {
        public List<ParseNode> children;
        public string value;
        public TaggedWord word;
    }
}
