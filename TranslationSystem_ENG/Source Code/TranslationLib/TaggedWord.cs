using System.Collections.Generic;

namespace TranslationLib
{
    public class TaggedWord
    {
        public string Word { get; set; }
        public Tag Category { get; set; }
        public string Tag_TableColumn { get; set; }
        public string POSTag = "";
    }

    public enum Tag
    {
       Value,
       Object
    }

    public class ValueTag
    {
        public string Value { get; set; }
        public string Column { get; set; }
    }
}
