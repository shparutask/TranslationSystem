#encoding "utf8"

Home -> 'усадьба' | 'Усадьба' | 'усадьбы' | 'Усадьбы';
descr -> AnyWord<gram="persn"> | AnyWord<gram="famn">;
HomesteadDescr -> (Adj<gram="род">) descr (AnyWord<gram="род">);

Homestead -> Home HomesteadDescr interp (Homestead.HomesteadName::not_norm); 