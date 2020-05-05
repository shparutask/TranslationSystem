import random
import numpy as np
from train_batch import Training
from encoder import Encoder
import torch
from vocab import Vocabluary
import torch.nn.functional as F
from gensim.models import Word2Vec

def get_col_emb(d_out):
    return np.random.uniform(0, 1, (len(Vocabluary.V_COL), d_out)) #np.matrix(np.array(U_COL_list).reshape(len(U_COL_list), d_out))

query = ['select']

#initialization lstm model
train = Training()
best_lstm = train.get_model_lstm()
best_lstm.batch_size = 1    
best_lstm.hidden = best_lstm.init_hidden()

question = 'какие музеи находятся на рубинштейна'

#get hidden states from encoder
h = Encoder().encode(question)
N = len(question.split())

sql_emb = np.random.uniform(0, 1, (len(Vocabluary.V_SQL), len(Vocabluary.V_SQL)))

t = 0
max_P = 1

s = sql_emb[0]

while max_P > 0.5 and t < 100:
    s_0 = s
    y_0 = best_lstm(torch.from_numpy(np.array(s_0, dtype=np.int64).reshape(len(s_0), 1))).data.T     

    y_0 /= y_0.max()

    #get attention
    a_0 = []
    for hidden_state in h:
        a_0.append(np.dot(hidden_state, y_0)[0][0])

    alpha_0 = F.softmax(torch.from_numpy(np.array(a_0)), dim=0)
    h_context_0 = []

    for i in range(0, N, 1):
        h_sum = 0
        for j in range(0, h[i].size - 1, 1):
            h_sum = h_sum + alpha_0[i] * h[i][0][j]
        h_context_0.append(np.array(h_sum).reshape(1, 1)[0][0])

    d_out = len(y_0) + len(h_context_0)
    U_SQL = np.random.uniform(0, 1, (len(Vocabluary.V_SQL), d_out))

    Y = list(np.array(y_0).reshape(len(y_0))) + h_context_0

    O_SQL = np.matrix(U_SQL) * np.matrix(np.array(Y).reshape(d_out, 1))
    
    U_COL = get_col_emb(d_out)
    O_COL = U_COL* np.matrix(np.array(Y).reshape(d_out, 1))
        
    O_E = []
    for word in train.text_field.vocab.itos:
        if word in question.split():
            O_E.append(np.max(a_0))
        else:
            O_E.append(float('-inf'))
            
    P = F.softmax(torch.from_numpy(np.array(list(np.array(O_SQL).reshape(len(Vocabluary.V_SQL)))+list(np.array(O_COL).reshape(O_COL.size))+list(O_E))), dim=0)
    max_index = P.argmax()
    max_P = max(P)

    if max_index < len(Vocabluary.V_SQL):
        sql_token = Vocabluary.V_SQL[max_index]
        s = sql_emb[max_index]
    if max_index >= len(Vocabluary.V_SQL) and max_index < (len(Vocabluary.V_SQL) + O_COL.size):
        sql_token = Vocabluary.V_COL[max_index - len(Vocabluary.V_SQL)][0]
        s = U_COL[max_index - len(Vocabluary.V_SQL)]
    if max_index >= (len(Vocabluary.V_SQL) + O_COL.size):
        sql_token = train.text_field.vocab.itos[max_index - len(Vocabluary.V_SQL) - O_COL.size]
        s = np.array(train.text_field.vocab.stoi[sql_token], dtype=np.int64).reshape(1, 1)
   
    query.append(sql_token)

    t += 1