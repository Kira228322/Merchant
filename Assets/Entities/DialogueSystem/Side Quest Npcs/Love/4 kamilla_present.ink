INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "love_failed_give_present"):
-> fail
}
{contains(activeQuests, "love_successful_give_present"):
->success
-else:
    ->generic
}


=== fail ===
Снова ты? Что тебе ещё нужно от меня?
	+[В соседней деревне живёт хороший парень по имени Микаэль. Вот подарок от него.]
		О боже, фу, что это? Какая мерзость! Ты и твой Микаэль хотите с ума меня свести?
		Вон отсюда! Микаэлю этому передай, что глаза мои его видеть не хотят! Какой кошмар!
		~invoke_dialogue_event("love_failed_give_present")
		~add_quest("love_report_fail")
		->END
		
=== success ===
О, снова привет. Ты чего-то хотел?
	+[В соседней деревне живёт парень по имени Микаэль. Вот подарок от него. *Отдать мармелад*]
		Ого, это же мой любимый мармелад! Как здорово!
		Микаэль, говоришь? Кажется, я видела его на какой-то ярмарке. А он похож на хорошего человека, я не прочь познакомиться с ним…
		~invoke_dialogue_event("love_successful_give_present")
		Спасибо, что передал подарок!
		->END

=== generic ===
~temp activeQuests = get_activeQuestList()
{contains (activeQuests, "love_report_fail"):
    Я сказала, убирайся с моих глаз! Видеть тебя не хочу! Уходи, или я позову стражу!
    ->END
-else:
    Спасибо тебе и Микаэлю за подарок, мармелад очень вкусный! М-м!
    ->END
}
