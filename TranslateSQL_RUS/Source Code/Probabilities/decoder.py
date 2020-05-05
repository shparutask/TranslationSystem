import random
import numpy as np
from train_batch import Training
from encoder import Encoder
import torch
from vocab import Vocabluary
import torch.nn.functional as F
from gensim.models import Word2Vec

question = 'какие музеи находятся на рубинштейна'
#get hidden states from encoder
h = Encoder().encode(question)
N = len(question.split())

#initialization lstm model
train = Training()
best_lstm = train.get_model_lstm()
best_lstm.batch_size = 1    
best_lstm.hidden = best_lstm.init_hidden()
pretrained_embeddings = []

#zero-state from decoder
s_0 = np.random.uniform(1, len(Vocabluary.V_SQL), (10, 1))       
y_0 = best_lstm(torch.from_numpy(np.array(s_0, dtype=np.int64))).data.T     

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

U_COL_list = []
columns = []
index = 0
zeros = [0, 0, 0, 0, 0]

for word in train.text_field.vocab.itos:
    for column in Vocabluary.V_COL:
        if column[0] not in columns and word in column:
            col_encode = best_lstm(torch.from_numpy(np.array(index, dtype=np.int64).reshape(1, 1))).data
            U_COL_list.append(np.array(zeros + list(np.array(col_encode).reshape(len(col_encode.T)))))
            columns.append(column[0])
            index = index + 1

U_COL = np.matrix(np.array(U_COL_list).reshape(len(U_COL_list), d_out))
O_COL = U_COL* np.matrix(np.array(Y).reshape(d_out, 1))
        
O_E = []
for word in train.text_field.vocab.itos:
    if word in question.split():
        O_E.append(np.max(a_0))
    else:
        O_E.append(float('-inf'))

P = F.softmax(torch.from_numpy(np.array(list(np.array(O_SQL).reshape(10))+list(np.array(O_COL).reshape(20))+list(O_E))), dim=0)

print('first output token') 