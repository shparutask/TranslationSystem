namespace TranslationLib
{
    public class TagWords
    {
        public string Word { get; set; }
        public Tag Category { get; set; }
        public string Tag { get; set; }
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
