#encoding "utf8"

HouseDescr -> 'дом' | 'д';
Num_H -> AnyWord<wff=/[1-9][0-9]*/>;

HouseNumber -> Num_H | HouseDescr Num_H | HouseDescr Num_H;

StreetW -> 'проспект' | 'проезд' | 'улица' | 'шоссе' | 'площадь'| 'набережная';
StreetSokr -> 'пр' | 'просп' | 'пр-д' | 'ул' | 'ш' | 'пл' | 'наб' | 'г';

StreetDescr -> StreetW | StreetSokr (Punct);

StreetNameNoun -> (Adj<gnc-agr[1]>) Word<gnc-agr[1],rt> (Word<gram="род">) | Adj<gram="loc"> | Adj<gram="nom">;

NumberW_1 -> AnyWord<wff=/[1-9]?[0-9]-?((ый)|(ий)|(ой)|й)/> {outgram="муж,ед,им"};
NumberW_2 -> AnyWord<wff=/[1-9]?[0-9]-?((ая)|(яя)|(ья)|я)/> {outgram="жен,ед,им"};
NumberW_3 -> AnyWord<wff=/[1-9]?[0-9]-?((ее)|(ье)|(ое)|е)/> {outgram="сред,ед,им"};

NumberW -> NumberW_1 | NumberW_2 | NumberW_3;

StreetNameAdj -> Adj<h-reg1> Adj* | NumberW<gnc-agr[1]> Adj<gnc-agr[1]>;

Street -> StreetDescr StreetNameNoun<h-reg1> interp (Address.StreetName);
Street -> StreetDescr StreetNameNoun<gram="род", h-reg1> interp (Address.StreetName::not_norm)',' HouseNumber interp (Address.HouseNumber);
Street -> StreetDescr StreetNameNoun<gram="им", h-reg1> interp (Address.StreetName::not_norm)',' HouseNumber interp (Address.HouseNumber);
Street -> StreetNameAdj<gnc-agr[1]> interp (Address.StreetName) StreetW<gnc-agr[1]>',' HouseNumber interp (Address.HouseNumber);
Street -> StreetNameAdj interp (Address.StreetName) StreetSokr',' HouseNumber interp (Address.HouseNumber);
//Выше мы описали только цепочки название - дескриптор, но в некоторых адресах порядок другой. 
//Добавляем правила для адресов с дескриптором, идущим перед названием улицы.
Street -> StreetW<gnc-agr[1]> StreetNameAdj<gnc-agr[1]> interp (Address.StreetName)',' HouseNumber interp (Address.HouseNumber);
Street -> StreetNameAdj interp (Address.StreetName)',' HouseNumber interp (Address.HouseNumber);