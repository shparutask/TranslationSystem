 # File: pos_tagging.py
# Template file for Informatics 2A Assignment 2:
# 'A Natural Language Query System in Python/NLTK'

# John Longley, November 2012
# Revised November 2013 and November 2014 with help from Nikolay Bogoychev
# Revised November 2015 by Toms Bergmanis and Shay Cohen


# PART B: POS tagging

from statements import *

# The tagset we shall use is:
# P  A  Ns  Np  Is  Ip  Ts  Tp  BEs  BEp  DOs  DOp  AR  AND  WHO  WHICH  ?

# Tags for words playing a special role in the grammar:

function_words_tags = [('a','AR'), ('an','AR'), ('and','AND'),
     ('is','BEs'), ('are','BEp'), ('does','DOs'), ('do','DOp'),
     ('who','WHO'), ('which','WHICH'), ('Who','WHO'), ('Which','WHICH'), ('?','?')]
     # upper or lowercase tolerated at start of question.

function_words = [p[0] for p in function_words_tags]

def unchanging_plurals():
    with open("sentences.txt", "r") as f:
        arrayNN = []
        arrayNNS = []
        r = []
        for line in f:
            for tagged in line.split():
                (word, tag) = tagged.split("|")
                if tag == 'NN' and word not in arrayNN:
                    arrayNN.append(word)
                if tag == 'NNS' and word not in arrayNNS:
                    arrayNNS.append(word)
        for word in arrayNNS:
            if word in arrayNN:
                r.append(word)
        return r

unchanging_plurals_list = unchanging_plurals()
#print unchanging_plurals_list

def noun_stem (s):
    """extracts the stem from a plural noun, or returns empty string"""
    if s in unchanging_plurals_list:
        return s
    elif (re.match("\w*man", s)):
        return s[:-2]+"en"
    elif (re.match("\w*([^aeiousxyzh]|[^cs]h)s$", s)):
        return s[:-1]
    elif (re.match("(\w*)[aeiou]ys$", s)):
        return s[:-1]
    elif (re.match("\w+[^aeiou]ies$", s)):
        return s[:-3]+'y'
    elif (re.match("\w*[^aeiou]ies$", s)):
        return s[:-1]
    elif (re.match("\w*([ox]|ch|sh|ss|zz)es$", s)):
        return s[:-2]
    elif (re.match("\w*(([^s]se)|([^z]ze))s$", s)):
        return s[:-1]
    elif (re.match("\w*([^iosxzh]|[^cs]h)es$", s)):
        return s[:-1]
    else:
        return ""

#print noun_stem("actresses")
#print noun_stem("countries")
#print noun_stem("fish")
#print noun_stem("celebrities")

def tag_word (lx,wd):
    """returns a list of all possible tags for wd relative to lx"""
    r = []
    for word in lx.getAll('N'):
        if noun_stem(wd) == word:
            r.append('Np')
        if wd == word:
            r.append('Ns')
    for word in lx.getAll('I'):
        if verb_stem(wd) == word:
            r.append('Is')
        elif wd == word:
            r.append('Ip')
    for word in lx.getAll('T'):
        if verb_stem(wd) == word:
            r.append('Ts')
        elif wd == word:
            r.append('Tp')
    for word in lx.getAll('A'):
        if wd == word:
            r.append('A')
    for word in lx.getAll('P'):
        if wd == word:
            r.append('P')
    for (word, tag) in function_words_tags:
            if (wd == word):
                r.append(tag)
    return r

#lx = Lexicon()
#lx.add("John", "P")
#lx.add("Mary", "P")
#lx.add("orange", "N")
#lx.add("orange", "A")
#lx.add("fish", "N")
#lx.add("fish", "I")
#lx.add("fish", "T")
#lx.add ("Who", "WHO")
#print tag_word(lx, "Who")
#print tag_word(lx, "fish")
#print tag_word(lx, "orange")
#print tag_word(lx, "John")
#print tag_word(lx, "asdfg")
#wlist=["Which","cars","John","drives","shine","?"]
#lx.add("car","N")
#lx.add("John","P")
#lx.add("drive","T")
#lx.add("Mary","P")
#lx.add("shine","I")
#print tag_word(lx, "cars")


def tag_words (lx, wds):
    """returns a list of all possible taggings for a list of words"""
    if (wds == []):
        return [[]]
    else:
        tag_first = tag_word (lx, wds[0])
        tag_rest = tag_words (lx, wds[1:])
        return [[fst] + rst for fst in tag_first for rst in tag_rest]

#print tag_words(lx, ["Who", "John", "fish"])
#for word in wlist:
#    tag_word(lx, word)
#print tag_words(lx, wlist)

# End of PART B.
