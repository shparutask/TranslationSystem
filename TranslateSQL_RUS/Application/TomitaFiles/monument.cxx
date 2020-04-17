#encoding "utf8"

Monum -> 'памятник' | 'Памятник' | 'памятника' | 'Памятника';
descr -> AnyWord<gram="persn"> | AnyWord<gram="famn">;
MonumDescr -> Adj<h-reg1> Adj* | (Adj<gram="род">) Noun<gram="род"> | (AnyWord<gram="дат">) descr (AnyWord<gram="дат">);

Monument -> MonumDescr<gnc-agr[1]> interp (Monument.NAME) Monum<gnc-agr[1]> | Monum MonumDescr interp (Monument.NAME::not_norm); 