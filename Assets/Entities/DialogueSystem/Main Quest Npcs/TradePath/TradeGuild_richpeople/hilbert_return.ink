INCLUDE ../../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "return_to_hilbert"):
    ->return_to_hilbert
-else:
    ->generic
}

=== return_to_hilbert ===
Вот и ты. Удалось ли тебе договориться с рабочими?
    +[Да. В скором времени они приступят к ремону дорог.]
        ~invoke_dialogue_event("hilbert_approved")
        ~invoke_dialogue_event("return_to_hilbert")
        Хорошо. Можешь сказать Карлосу, что я даю своё одобрение.
            ->END


=== generic ===
Ну здравствуй, посетитель! Выбирай любые товары на свой вкус.
    +[Я пойду.]
        Приходи ещё!
            ->END

