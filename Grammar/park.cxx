#encoding "utf8"

Park -> 'парк' | 'Парк' | 'парка' | 'Парка';
ParkDescr -> Adj<gram="ед"> | (Adj<gram="род">) Noun<gram="род">;

P -> ParkDescr<gnc-agr[1]> interp (Park.NAME) Park<gnc-agr[1]> | Park ParkDescr interp (Park.NAME::not_norm);