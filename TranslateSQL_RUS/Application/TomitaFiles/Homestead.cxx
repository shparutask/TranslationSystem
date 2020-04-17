#encoding "utf8"

Home -> 'усадьба' | 'Усадьба' | 'усадьбы' | 'Усадьбы' | 'дворец' | 'Дворец' | 'дворца' | 'Дворца';
descr->AnyWord<gram = "persn"> | AnyWord<gram="famn">;
HomesteadDescr -> Adj<h-reg1> Adj* | (Adj<gram="род">) Noun<gram="род"> | (Adj<gram="род">) descr (AnyWord<gram="род">);

Homestead -> Home HomesteadDescr interp (Homestead.NAME::not_norm); 