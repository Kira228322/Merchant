INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "intercept_smuggled_goods_1"):
    ->interrogate
-else:
    ->generic
}

=== interrogate ===
\*Вы чувствуете резкий запах алкоголя. Ричард выглядит радостным, его глаза блестят.*
О-хо, покупатель! Заходи, заходи, дружище! Приятно познакомиться, я Ричард!
Чего желаешь приобрести?
    +[У тебя есть что-нибудь особенное? Лично для меня?]
        Ха-ха, я так и думал, что это вы! Сразу вас не узнал. Вот, подойдите поближе, товар здесь.
        \*Вы встаете к свету, и Ричард видит ваше лицо*
        Погоди-ка, не тебя я ждал... Ты ведь сын того дурака. Убирайся отсюда, немедленно!
            ++[Так значит, ты промышляешь контрабандой. Мне рассказать страже?]
                Нет, стой! Зараза. Угораздило же меня...
                    +++[Скажи мне, кто должен был прийти вместо меня, и я не сообщу страже.]
                        Ладно, ладно! Есть такой торговец из деревни Верхогорье, я не знаю его имени. Мы договорились, что он будет приходить ко мне и забирать часть товара...
                        ++++[Откуда ты берешь этот товар?]
                                ...
                                ....Я не могу сказать. Меня ведь убьют!
                                ~invoke_dialogue_event("intercept_smuggled_goods_1")
                                Не выдавай меня страже, прошу. Мне всего лишь нужны деньги.
                                    +++++[Я подумаю.]
                                        ...
                                            ->END


=== generic ===
Ну чего тебе ещё надо? Не хочешь торговать - убирайся.
    +[Я пойду.]
        Иди уже.
            ->END