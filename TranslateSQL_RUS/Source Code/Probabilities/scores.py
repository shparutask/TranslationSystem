from word2vec import Embedding
import pandas as pd
from gensim.models import Word2Vec

fname = "data\\dataset.tsv"
question = "сколько галерей находится"
emb_size = 250

def get_embeddings(dataset):        
    sentences = []
    pretrained_embeddings = []

    for line in dataset.values.T[0]:
        sentences.append(line.split())

    return Word2Vec(sentences, min_count=1, size = emb_size).wv.vocab

#get dataset
dataset = pd.read_csv(fname, delimiter='\t')
dataset.columns = ['question', 'query', 'count']
dataset['count'] /= sum(dataset['count'])

#get embeddings
vocab = get_embeddings(dataset);

dataset_prob_sort = dataset.sort_values(['count'], ascending= False)

propose_list = []
for cell in dataset_prob_sort.values:
    if question in cell[0]:
        propose_list.append(cell)

print(propose_list)