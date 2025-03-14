INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "love_failed_get_present"):
    -> fail
}
{contains(activeQuests, "love_successful_get_present"):
    -> success
-else:
    -> generic
}

=== fail ===
Ну как? Поговорил? Сказала ли она что-нибудь?
+[Да. Ей определённо нравится слизь.]
	Слизь? Как это странно… Ну, о вкусах не спорят! Мой отец – охотник на монстров, после охоты на слизней он часто приносит слизь домой. Правда, обычно мы её сжигаем в печи.
	    В общем, будь добр, возьми вот этот кусок и подари ей. Скажи, что это от меня.
	    ~invoke_dialogue_event("love_failed_get_present")
	    ~place_item("Слизь для Камиллы", 1, 0)
	    Ох, надеюсь, ей понравится!
	    ->END
+[Ещё нет.]
	Пожалуйста, я рассчитываю на тебя!
	->END
	
=== success ===

Ну как? Поговорил? Сказала ли она что-нибудь?
+[Да. Ей определённо нравится мармелад.]
	Вот как? Хм, по случайности, мне тут как раз недавно передали баночку. Ты не мог бы подарить её ей? Скажи, что это от меня.
	В общем, будь добр, возьми вот эту банку и подари ей. Скажи, что это от меня.
	~invoke_dialogue_event("love_successful_get_present")
	    ~place_item("Мармелад для Камиллы", 1, 0)
	Надеюсь, ей понравится!
	->END
+[Ещё нет.]
	Пожалуйста, я рассчитываю на тебя!
	->END
	
=== generic ===
Ох, интересно, понравится ли ей подарок... Не терпится узнать!
->END