using System.Collections.Generic;

namespace TranslationLib
{
    public class Lexicon
    {
        public List<ValueTag> value_tags { get; set; }
        public List<string>[] Language_tags { get { return language_tags; } }

        List<string>[] language_tags = new List<string>[] { new List<string>() { "MONUMENTS_OF_ARCHITECHTURE.NAME", "name", "architechture" },
                                                            new List<string>() { "MONUMENTS_OF_ARCHITECHTURE", "architechture", "monument"},
                                                            new List<string>() { "MONUMENTS_OF_ARCHITECHTURE.DESCRIPTION", "description"},

                                                            new List<string>() { "ADDRESSES.ID", "Address", "addresses"},
                                                            new List<string>() { "ADDRESSES.STREET", "street", "pr.", "st.", "prospekt"},
                                                            new List<string>() { "ADDRESSES.HOUSE_NUMBER", "house", "number"},

                                                            new List<string>() { "AREAS.NAME", "names", "areas", "name", "area", "district", "districts", "region", "regions"},

                                                            new List<string>() { "ESTATES.NAME", "name", "names"},
                                                            new List<string>() { "ESTATES.ID", "estate", "estates"},
                                                            new List<string>() { "ESTATES.DESCRIPTION", "description"},

                                                            new List<string>() { "MONUMENTS.NAME", "name", "monument", "monumets" },
                                                            new List<string>() { "MONUMENTS.ID", "monument", "monuments"},
                                                            new List<string>() { "MONUMENTS.DESCRIPTION", "description"},

                                                            new List<string>() { "MUSEUMS.NAME", "name", "names"},
                                                            new List<string>() { "MUSEUMS.ID", "museum", "museums"},
                                                            new List<string>() { "MUSEUMS.DESCRIPTION", "description"},
                                                            new List<string>() { "MUSEUMS.OPENING_AT", "open", "opened", "opening"},
                                                            new List<string>() { "MUSEUMS.CLOSING_AT", "close", "closing", "closed"},
                                                            new List<string>() { "MUSEUMS.WORKING_DAYS", "work", "working", "works", "day", "days"},

                                                            new List<string>() { "PARKS.NAME", "name", "names" },
                                                            new List<string>() { "PARKS.ID", "park", "parks"},
                                                            new List<string>() { "PARKS.DESCRIPTION", "description"},
                                                            new List<string>() { "PARKS.OPENING_AT", "open", "opened", "opening"},
                                                            new List<string>() { "PARKS.CLOSING_AT", "close", "closing", "closed"},
                                                            new List<string>() { "PARKS.WORKING_DAYS", "work", "working", "works", "day", "days"},
        };

        List<TaggedWord> lx = new List<TaggedWord>();

        public Lexicon(dbGraph g)
        {
            value_tags = g.ReturnDataValues();
        }

        public void Add(TaggedWord word)
        {
            lx.Add(word);
        }

        public List<TaggedWord> getAll()
        {
            return lx;
        }

        public void changeLx(List<TaggedWord> new_wlist)
        {
            lx = new_wlist;
        }
    }
}
