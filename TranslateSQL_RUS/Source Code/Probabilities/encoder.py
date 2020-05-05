from train_batch import Training 
from gensim.models import Word2Vec
import torch
import numpy as np

class Encoder:
    def encode(self, question):
        train = Training()
        best_bilstm = train.get_model()
        sent = []
        h = []
        best_bilstm.batch_size = 1    
        best_bilstm.hidden = best_bilstm.init_hidden()   

        for word in question.split():
            hidden_state = best_bilstm(torch.from_numpy(np.array(train.text_field.vocab.stoi[word], dtype=np.int64).reshape(1, 1)))
            h.append(np.array(hidden_state.data))
            
        return h
