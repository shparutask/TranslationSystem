from gensim.models import Word2Vec
import codecs

fname = 'C:\\Users\\sofis\\Desktop\\pytorch-sentiment-classification-master\\data\\questions.txt'

# define training data
sentences = []

with open(fname, "r", encoding="utf-8") as f:
    s = f.readline()
    while s:
        sentences.append(s.split())
        s = f.readline()

# train model
model = Word2Vec(sentences, min_count=1)

# summarize the loaded model
print(model)
# summarize vocabulary
words = list(model.wv.vocab)
print(words)
# access vector for one word
print(model['Сколько'])
# save model
model.save('C:\\Users\\sofis\\Desktop\\pytorch-sentiment-classification-master\\data\\model.bin')
# load model
new_model = Word2Vec.load('C:\\Users\\sofis\\Desktop\\pytorch-sentiment-classification-master\\data\\model.bin')
print(new_model)
