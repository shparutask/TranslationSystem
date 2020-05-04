import random
import numpy as np
from train_batch import Training
from encoder import Encoder
import torch

class Decoder:
    s = ['select']
    V_COL = ['ADDRESSES.*', 'ADDRESSES.ID', 'ADDRESSES.STREET', 'ADDRESSES.HOUSENUMBER', 'ADDRESSES.ID_AREA',\
             'AREAS.ID', 'AREAS.NAME',\
             'HOMESTEADS.ID', 'HOMESTEADS.NAME', 'HOMESTEADS.DESCRIPTION', 'HOMESTEADS.ID_ADDRESS',\
             'MONUMENTS.ID', 'MONUMENTS.NAME', 'MONUMENTS.DESCRIPTION', 'MONUMENTS.ID_ADDRESS',\
             'MUSEUMS.ID', 'MUSEUMS.NAME', 'MUSEUMS.DESCRIPTION', 'MUSEUMS.OPENING_HOURS', 'MUSEUMS.CLOSING_TIME', 'MUSEUMS.WORKING_DAYS', 'MUSEUMS.ID_ADDRESS',\
             'PARKS.ID', 'PARKS.NAME', 'PARKS.DESCRIPTION', 'PARKS.OPENING_HOURS', 'PARKS.CLOSING_TIME', 'PARKS.WORKING_DAYS', 'PARKS.ID_ADDRESS']
    V_SQL = ['select', 'from', 'where', 'and', 'or', 'count', '(*)', 'join', 'on', 'like']
   

    def decode(self): 
        U_SQL = np.random.uniform(0, 1, (len(self.V_SQL), 174))

        h = Encoder().encode('какие музеи находятся на рубинштейна')

        t = 0
        s_prev_emb = np.random.uniform(1, 10, (231, 1))

        train = Training()
        best_lstm = train.get_model_lstm()

        best_lstm.batch_size = 1    
        best_lstm.hidden = best_lstm.init_hidden()        
        y = best_lstm(torch.from_numpy(np.array(s_prev_emb, dtype=np.int64)))

        O_SQL = np.matrix(U_SQL)*np.matrix(np.array((y.data.T/h.data.T)))



        
        best_lstm(torch.from_numpy(np.array(sent, dtype=np.int64).reshape(len(sent), 1)))

        return


Decoder().decode() 