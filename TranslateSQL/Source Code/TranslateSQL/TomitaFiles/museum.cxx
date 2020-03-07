#encoding "utf8"

Ve -> 'работает';
Pre -> 'c';
Post -> 'по' | 'до';

hours -> Pre AnyWord<wff=/[0-2][0-9]:[0-5][0-9]/>;
hours2 -> Post AnyWord<wfl=/[0-2][0-9]:[0-5][0-9]/>;

Museum -> 'музей' | 'Mузей' | 'музея' | 'Mузея' | 'Музеи' | 'музеи';
MuseumDescr -> Adj<h-reg1> Adj* | (Adj<gram="род">) Noun<gram="род">;

Mus-> MuseumDescr<gnc-agr[1]> interp(Museum.NAME) Museum<gnc-agr[1]> Ve hours interp(Museum.OPENING_HOURS) hours2 interp(Museum.CLOSING_TIME) | MuseumDescr<gnc-agr[1]> interp(Museum.NAME) Museum<gnc-agr[1]> | Museum MuseumDescr interp(Museum.NAME::not_norm); 