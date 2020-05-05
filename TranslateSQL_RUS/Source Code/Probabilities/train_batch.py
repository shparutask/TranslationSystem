import torch
import torch.nn as nn
from torch import optim
import time
import random
import os
from tqdm import tqdm
from lstm import LSTMSentiment
from bilstm import BiLSTMSentiment
from torchtext import data
import numpy as np
import argparse
import codecs
from gensim.models import Word2Vec

torch.set_num_threads(8)
torch.manual_seed(1)
random.seed(1)

class Training():
    text_field = []
    label_field = []

    def load_bin_vec(self, fname, vocab):
        """
        Loads 300x1 word vecs from Google (Mikolov) word2vec
        """
        word_vecs = {}
        with open(fname, "rb") as f:
            header = f.readline()
            vocab_size, layer1_size = map(int, header.split())
            binary_len = np.dtype('float32').itemsize * layer1_size
            for line in range(vocab_size):
                word = []
                while True:
                    ch = f.read(1).decode('latin-1')
                    if ch == ' ':
                        word = ''.join(word)
                        break
                    if ch != '\n':
                        word.append(ch)
                if word in vocab:
                   word_vecs[word] = np.fromstring(f.read(binary_len), dtype='float32')
                else:
                    f.read(binary_len)
        return word_vecs

    def get_accuracy(self, truth, pred):
        assert len(truth) == len(pred)
        right = 0
        for i in range(len(truth)):
            if truth[i] == pred[i]:
                right += 1.0
        return right / len(truth)

    def train_epoch_progress(self, model, train_iter, loss_function, optimizer, text_field, label_field, epoch):
        model.train()
        avg_loss = 0.0
        truth_res = []
        pred_res = []
        count = 0
        for batch in tqdm(train_iter, desc='Train epoch ' + str(epoch + 1)):
            sent, label = batch.text, batch.label
            label.data.sub_(1)
            truth_res += list(label.data)
            model.batch_size = len(label.data)
            model.hidden = model.init_hidden()
            pred = model(sent)
            pred_label = pred.data.max(1)[1].numpy()
            pred_res += [x for x in pred_label]
            model.zero_grad()
            loss = loss_function(pred, label)
            if loss.data.ndim != 0:
                avg_loss += loss.data[0]
            count += 1
            loss.backward()
            optimizer.step()
        avg_loss /= len(train_iter)
        acc = self.get_accuracy(truth_res, pred_res)
        return avg_loss, acc

    def train_epoch(self, model, train_iter, loss_function, optimizer):
        model.train()
        avg_loss = 0.0
        truth_res = []
        pred_res = []
        count = 0
        for batch in train_iter:
            sent, label = batch.text, batch.label
            label.data.sub_(1)
            truth_res += list(label.data)
            model.batch_size = len(label.data)
            model.hidden = model.init_hidden()
            pred = model(sent)
            pred_label = pred.data.max(1)[1].numpy()
            pred_res += [x for x in pred_label]
            model.zero_grad()
            loss = loss_function(pred, label)
            avg_loss += loss.data[0]
            count += 1
            loss.backward()
            optimizer.step()
        avg_loss /= len(train_iter)
        acc = self.get_accuracy(truth_res, pred_res)
        return avg_loss, acc

    def evaluate(self, model, data, loss_function, name):
        model.eval()
        avg_loss = 0.0
        truth_res = []
        pred_res = []
        for batch in data:
            sent, label = batch.text, batch.label
            label.data.sub_(1)
            truth_res += list(label.data)
            model.batch_size = len(label.data)
            model.hidden = model.init_hidden()
            pred = model(sent)
            pred_label = pred.data.max(1)[1].numpy()
            pred_res += [x for x in pred_label]
            loss = loss_function(pred, label)
            if loss.data.ndim != 0:
                avg_loss += loss.data[0]
        avg_loss /= len(data)
        acc = self.get_accuracy(truth_res, pred_res)
        print(name + ': loss %.2f acc %.1f' % (avg_loss, acc * 100))
        return acc

    def load_sst(self, text_field, label_field, batch_size):
        train, dev, test = data.TabularDataset.splits(path='C:\\Users\\sofis\\Desktop\\12 семестр\\ВКР\\TranslationSystem\\TranslateSQL_RUS\\Source Code\\pytorch-sentiment-classification-master\\data\\SST2\\', train='train_q.tsv',
                                                      validation='dev_q.tsv', test='test_q.tsv', format='tsv',
                                                      fields=[('text', text_field), ('label', label_field)])
        text_field.build_vocab(train, dev, test)
        label_field.build_vocab(train, dev, test)
        train_iter, dev_iter, test_iter = data.BucketIterator.splits((train, dev, test),
                    batch_sizes=(batch_size, len(dev), len(test)), sort_key=lambda x: len(x.text), repeat=False, device=-1)
        return train_iter, dev_iter, test_iter

    def get_model(self):
        EPOCHS = 20
        USE_GPU = torch.cuda.is_available()
        EMBEDDING_DIM = 231
        HIDDEN_DIM = 150

        BATCH_SIZE = 19
        timestamp = str(int(time.time()))
        best_dev_acc = 0.0

        self.text_field = data.Field(lower=True)
        self.label_field = data.Field(sequential=False)
        train_iter, dev_iter, test_iter = self.load_sst(self.text_field, self.label_field, BATCH_SIZE)

        model = BiLSTMSentiment(embedding_dim=EMBEDDING_DIM, hidden_dim=HIDDEN_DIM, vocab_size=len(self.text_field.vocab), label_size=len(self.label_field.vocab) - 1,\
                              use_gpu=USE_GPU, batch_size=BATCH_SIZE)

        if USE_GPU:
            model = model.cuda()

        print('Load word embeddings...')

        pretrained_embeddings = []
        fname = 'C:\\Users\\sofis\\Desktop\\12 семестр\\ВКР\\TranslationSystem\\TranslateSQL_RUS\\Source Code\\pytorch-sentiment-classification-master\\data\\questions.txt'

    # define training data
        sentences = []

        with open(fname, "r", encoding="utf-8") as f:
            s = f.readline()
            while s:
                sentences.append(s.split())
                s = f.readline()

    # train model
        word2vec = Word2Vec(sentences, min_count=1, size = 286)

    # summarize vocabulary
        words = list(word2vec.wv.vocab)

        for word in words:
            pretrained_embeddings.append(word2vec[word])

        model.embeddings.weight.data.copy_(torch.from_numpy(np.array(pretrained_embeddings).T))
        best_model = model
        optimizer = optim.Adam(model.parameters(), lr=1e-5)
        loss_function = nn.NLLLoss()

        print('Training...')
        out_dir = os.path.abspath(os.path.join(os.path.curdir, "runs", timestamp))
        print("Writing to {}\n".format(out_dir))
        if not os.path.exists(out_dir):
            os.makedirs(out_dir)
        for epoch in range(EPOCHS):
            avg_loss, acc = self.train_epoch_progress(model, train_iter, loss_function, optimizer, self.text_field, self.label_field, epoch)
            tqdm.write('Train: loss %.2f acc %.1f' % (avg_loss, acc * 100))
            dev_acc = self.evaluate(model, dev_iter, loss_function, 'Dev')
            if dev_acc > best_dev_acc:
                if best_dev_acc > 0:
                        os.system('rm ' + out_dir + '/best_model' + '.pth')
                best_dev_acc = dev_acc
                best_model = model
                torch.save(best_model.state_dict(), out_dir + '/best_model' + '.pth')
            # evaluate on test with the best dev performance model
                test_acc = self.evaluate(best_model, test_iter, loss_function, 'Test')
        test_acc = self.evaluate(best_model, test_iter, loss_function, 'Final Test')
        return best_model
    
    def get_model_lstm(self):

        EPOCHS = 20
        USE_GPU = torch.cuda.is_available()
        EMBEDDING_DIM = 231
        HIDDEN_DIM = 150

        BATCH_SIZE = 19
        timestamp = str(int(time.time()))
        best_dev_acc = 0.0

        self.text_field = data.Field(lower=True)
        self.label_field = data.Field(sequential=False)
        train_iter, dev_iter, test_iter = self.load_sst(self.text_field, self.label_field, BATCH_SIZE)

        model = LSTMSentiment(embedding_dim=EMBEDDING_DIM, hidden_dim=HIDDEN_DIM, vocab_size=len(self.text_field.vocab), label_size=len(self.label_field.vocab)-1, use_gpu=USE_GPU, batch_size=BATCH_SIZE)

        if USE_GPU:
            model = model.cuda()

        print('Load word embeddings...')

        word_to_idx = self.text_field.vocab.stoi

        pretrained_embeddings = []
        fname = 'C:\\Users\\sofis\\Desktop\\12 семестр\\ВКР\\TranslationSystem\\TranslateSQL_RUS\\Source Code\\pytorch-sentiment-classification-master\\data\\questions.txt'

    # define training data
        sentences = []

        with open(fname, "r", encoding="utf-8") as f:
            s = f.readline()
            while s:
                sentences.append(s.split())
                s = f.readline()

    # train model
        word2vec = Word2Vec(sentences, min_count=1, size = 286)

    # summarize vocabulary
        words = list(word2vec.wv.vocab)

        for word in words:
            pretrained_embeddings.append(word2vec[word])

        model.embeddings.weight.data.copy_(torch.from_numpy(np.array(pretrained_embeddings).T))
        best_model = model
        optimizer = optim.Adam(model.parameters(), lr=1e-5)
        loss_function = nn.NLLLoss()

        print('Training...')
        out_dir = os.path.abspath(os.path.join(os.path.curdir, "runs", timestamp))
        print("Writing to {}\n".format(out_dir))
        if not os.path.exists(out_dir):
            os.makedirs(out_dir)
        for epoch in range(EPOCHS):
            avg_loss, acc = self.train_epoch_progress(model, train_iter, loss_function, optimizer, self.text_field, self.label_field, epoch)
            tqdm.write('Train: loss %.2f acc %.1f' % (avg_loss, acc * 100))
            dev_acc = self.evaluate(model, dev_iter, loss_function, 'Dev')
            if dev_acc > best_dev_acc:
                if best_dev_acc > 0:
                        os.system('rm ' + out_dir + '/best_model' + '.pth')
                best_dev_acc = dev_acc
                best_model = model
                torch.save(best_model.state_dict(), out_dir + '/best_model' + '.pth')
            # evaluate on test with the best dev performance model
                test_acc = self.evaluate(best_model, test_iter, loss_function, 'Test')
        test_acc = self.evaluate(best_model, test_iter, loss_function, 'Final Test')
        return best_model

