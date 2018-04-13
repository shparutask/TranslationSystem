using System.Collections.Generic;

namespace TranslationLib
{
    public class Lexicon
    {
        public List<ValueTag> value_tags { get; set; }
        public List<string>[] Language_tags { get { return language_tags; } }

        List<string>[] language_tags = new List<string>[] { new List<string>() {"FIRST_NAME.NAME_ENG", "name" },

                                                            new List<string>() {"PUBLICATION", "publication", "publications", "work"},
                                                            new List<string>() {"PUBLICATION.TITLE", "title"},
                                                            new List<string>() {"PUBLICATION.DESCRIPTION_ENG", "description"},
                                                            new List<string>() {"PUBLICATION.REFERENCE", "reference", "references"},

                                                            new List<string>() {"E_MAIL.MAIL", "mail", "e-mail", "E_mail", "mails", "emails" },

                                                            new List<string>() {"TELEPHONE.PHONE", "phone", "telephone", "phones"},

                                                            new List<string>() {"EXPERT", "author", "expert", "experts", "authors" },
                                                            new List<string>() {"EXPERT.DEGREE", "degree" },
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
