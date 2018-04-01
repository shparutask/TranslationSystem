using System.Collections.Generic;

namespace TranslationLib
{
    public class Lexicon
    {
        public List<ValueTag> value_tags { get; set; }
        public List<string>[] Language_tags { get { return language_tags; } }

        List<string>[] language_tags = new List<string>[] { new List<string>() {"LAST_NAME", "name" },
                                                            new List<string>() {"FIRST_NAME", "name" },
                                                            new List<string>() {"PUBLICATION", "publication"},
                                                            new List<string>() {"E_MAIL.MAIL", "mail", "e-mail", "E_mail" },
                                                            new List<string>() {"TELEPHONE", "phone"},
                                                            new List<string>() {"EXPERT_PUBLICATION" },
                                                            new List<string>() {"MIDDLE_NAME"},
                                                            new List<string>() {"EXPERT" },
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
