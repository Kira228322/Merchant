INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()
{contains(activeQuests, "beggining_of_valor_2"):
    ->quest
-else:
    ->generic
}
=== quest ===
Привет, ну как? Поговорил с жителями?
+[Да, одного из них нужно сопроводить в соседнюю деревню за лекарем. Справишься?]
	Хах, конечно! С этим новым оружием и бронёй никакие опасности мне не страшны! Спасибо, что нашёл мне задание!
        ~invoke_dialogue_event("beggining_of_valor_2_complete")	
        Ладно, мне пора бежать. Ещё увидимся!
->END

=== generic ===

Что скажешь?
    +[Я пойду.]
        Удачи!
            ->END
