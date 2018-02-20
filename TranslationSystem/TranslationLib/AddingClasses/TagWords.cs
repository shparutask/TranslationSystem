namespace TranslationLib
{
    class TagWords
    {
        public string word { get; set; }
        public Tags tag { get; set; }
    }

    public enum Tags
    {
        Table, 
        Column,
        Value,
        AR,
        AND,
        BEs,
        BEp,
        DOs,
        DOp,
        WHO,
        WHICH,
        Q,
        Another
    }
}
