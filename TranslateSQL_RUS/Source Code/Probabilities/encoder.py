from train_batch import Training 
from gensim.models import Word2Vec
import torch
import numpy as np

class Encoder:
    def encode(self, question):
        train = Training()
        best_bilstm = train.get_model()
        sent = []

        for word in question.split():
            sent.append(train.text_field.vocab.stoi[word])

        best_bilstm.batch_size = 1    
        best_bilstm.hidden = best_bilstm.init_hidden()        
        
        
        return best_bilstm(torch.from_numpy(np.array(sent, dtype=np.int64).reshape(len(sent), 1)))
