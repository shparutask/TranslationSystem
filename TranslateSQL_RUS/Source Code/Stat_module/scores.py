import pandas as pd
import sys

fname = "C:\\Users\\sofis\\Desktop\\12 семестр\\ВКР\\TranslationSystem\\TranslateSQL_RUS\\Source Code\\Stat_module\\data\\dataset.tsv"

def get_proposes(question):
    dataset = pd.read_csv(fname, delimiter='\t')
    dataset.columns = ['question', 'query', 'count']
    dataset['count'] /= sum(dataset['count'])

    propose_list = []
    for cell in dataset.sort_values(['count'], ascending= False).values:
        if question in cell[0]:
            propose_list.append(cell[0])

    return propose_list

print(get_proposes(sys.argv[1]))