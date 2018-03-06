import re
from nltk.corpus import brown
import sys


def verb_stem(s):
    ok = 0
    if (re.match("\w*([^aeiousxyzh]|[^cs]h)s$", s)):
        stem = s[:-1]
    elif (re.match("(\w*)[aeiou]ys$", s)):
        stem = s[:-1]
    elif (re.match("\w+[^aeiou]ies$", s)):
        stem = s[:-3]+'y'
    elif (re.match("[^aeiou]ies$", s)):
        stem = s[:-1]
    elif (re.match("\w*([ox]|ch|sh|ss|zz)es$", s)):
        stem = s[:-2]
    elif (re.match("\w*(([^s]se)|([^z]ze))s$", s)):
        stem = s[:-1]
    elif (re.match("has", s)):
        stem = "have"
    elif (re.match("\w*([^iosxzh]|[^cs]h)es$", s)):
        stem = s[:-1]
    else:
        stem = ""

    if (stem != "" and ok != 1):
        for (word, tag) in brown.tagged_words():
            if word == stem and tag in ('VB', 'VBZ'):
                print(stem)
                ok = 1
                break

    if (ok == 0):
        print("")

if __name__ == '__main__':
    verb_stem(sys.argv[1])