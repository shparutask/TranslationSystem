#encoding "utf8"

Museum -> 'музей' | 'Mузей' | 'музея' | 'Mузея';
MuseumDescr -> Adj<h-reg1> Adj* | (Adj<gram="род">) Noun<gram="род">;

Mus -> MuseumDescr<gnc-agr[1]> interp (Museum.MuseumName) Museum<gnc-agr[1]> | Museum MuseumDescr interp (Museum.MuseumName::not_norm); 