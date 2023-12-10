INCLUDE ../../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "return_to_oswald"):
    ->return_to_oswald
-else:
    ->generic
}

=== return_to_oswald ===
Что, уже вернулся? Получил ли одобрение у властей?
    +[Да. Мою заявку в итоге приняли.]
        ~invoke_dialogue_event("oswald_approved")
        ~invoke_dialogue_event("return_to_oswald")
        Ну тогда иди отсюда поскорее. Передай Карлосу, что я даю своё одобрение.
            ->END


=== generic ===
    Ты что, снова пришёл вести со мной какие-то дела? Ни о чём, кроме торговли, я и говорить не хочу!
        +[Тогда я пойду.]
            \*Освальд смотрит вам вслед злобным взглядом, в котором перемешались страх и гнев.*
                ->END
