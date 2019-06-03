#encoding "utf8"

Monum -> 'памятник' | 'Памятник' | 'памятника' | 'Памятника';
descr -> AnyWord<gram="persn"> | AnyWord<gram="famn">;
MonumDescr -> (Adj<gram="дат">) descr (AnyWord<gram="дат">);

Monument -> Monum MonumDescr interp (Monument.MonumentName::not_norm); 