#encoding "utf8"

MuseumWord -> 'музей' | 'Mузей' | 'музея' | 'Mузея' | 'Музеи' | 'музеи';
MuseumDescr -> Adj<h-reg1> Adj* | (Adj<gram="род">) Noun<gram="род">;

Mus -> MuseumDescr<gnc-agr[1]> interp(Museum.NAME) MuseumWord<gnc-agr[1]>;
Mus -> MuseumWord MuseumDescr interp(Museum.NAME::not_norm);