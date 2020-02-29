#encoding "utf8"

Pre -> 'c';
Post -> 'по' | 'до';

hours -> AnyWord<wff=/[0-2][0-9]:[0-5][0-9]/>;
hours2 -> AnyWord<wfl=/[0-2][0-9]:[0-5][0-9]/>;

Museum -> 'музей' | 'Mузей' | 'музея' | 'Mузея';
MuseumDescr -> Adj<h-reg1> Adj* | (Adj<gram="род">) Noun<gram="род">;

Mus -> MuseumDescr<gnc-agr[1]> interp (Museum.NAME) Museum<gnc-agr[1]> Verb Pre hours interp (Museum.OPENING_HOURS) Post hours2 interp (Museum.CLOSING_TIME);
// | MuseumDescr<gnc-agr[1]> interp (Museum.NAME) Museum<gnc-agr[1]> | Museum MuseumDescr interp (Museum.NAME::not_norm); 