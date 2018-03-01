import re

def unchanging_plurals():
    with open("C:/Users/Sonya/Desktop/sentences.txt", "r") as f:
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