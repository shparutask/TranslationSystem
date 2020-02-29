#encoding "utf8"

Initial -> Word<wff=/[А-Я]\./>;

Initials -> Initial Initial;

FIO -> Initials Word<kwtype=surname>;

Imeni -> 'они'<gram="дат"> FIO;