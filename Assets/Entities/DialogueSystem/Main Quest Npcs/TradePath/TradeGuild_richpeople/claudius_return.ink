INCLUDE ../../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "return_to_claudius"):
    ->return_to_claudius
-else:
    ->generic
}

=== return_to_claudius ===
Вот и ты. Как я понимаю, таверна открыта?
    +[Да. Оскар уже получает первую прибыль.]
        Замечательно! Скоро я и сам туда отправлюсь, отведать местной выпивки.
        ~invoke_dialogue_event("claudius_approved")
        ~invoke_dialogue_event("return_to_claudius")
        Передай Карлосу, что я считаю, что этот торговый путь ждёт отличное будущее! 
            ->END


=== generic ===
Ну здравствуй, посетитель! Выбирай любые товары на свой вкус.
    +[Я пойду.]
        Приходи ещё!
            ->END
