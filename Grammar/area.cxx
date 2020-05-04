#encoding "utf8"

AreaWord -> 'район' | 'Район' | 'района' | 'районе';
AreaDescr -> Adj<gnc-agr[1]>;

Area -> AreaDescr<gnc-agr[1]> interp(Area.NAME) AreaWord;