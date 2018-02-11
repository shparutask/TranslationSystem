from semantics import *
from statements import *
import sys

lx = Lexicon()
lx.add("fly", "I")
lx.add("swim", "I")
lx.add("John", "P")
lx.add("Mary", "P")
lx.add("duck", "N")
lx.add("student", "N")
lx.add("hit", "T")
lx.add("like", "T")
lx.add("purple", "A")
lx.add("old", "A")
print (lx.getAll("I"))

fb = FactBase()
fb.addUnary("duck","John")
fb.addBinary("love","John","Mary")
print (fb.queryUnary('duck','John'))
print (fb.queryBinary("love","Mary","John"))

print (verb_stem("boxes"))
print (verb_stem("bathes"))

lx.add("John", "P")
lx.add("Mary", "P")
lx.add("orange", "N")
lx.add("orange", "A")
lx.add("fish", "N")
lx.add("fish", "I")
lx.add("fish", "T")
lx.add ("Who", "WHO")
print (tag_word(lx, "Who"))
print (tag_word(lx, "fish"))
print (tag_word(lx, "orange"))
print (tag_word(lx, "John"))
print (tag_word(lx, "asdfg"))
wlist=["Which","cars","John","drives","shine","?"]
lx.add("car","N")
lx.add("John","P")
lx.add("drive","T")
lx.add("Mary","P")
lx.add("shine","I")
#print (tag_word(lx, "cars"))

dialogue(lx, fb)