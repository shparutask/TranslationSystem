#encoding "utf8"

Park -> 'парк' | 'Парк' | 'парка' | 'Парка';
ParkDescr -> Adj<h-reg1> Adj* | (Adj<gram="род">) Noun<gram="род">;

P -> ParkDescr<gnc-agr[1]> interp (Park.ParkName) Park<gnc-agr[1]> | Park ParkDescr interp (Park.ParkName::not_norm);