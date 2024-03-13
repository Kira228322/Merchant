INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList_universal()

{contains(activeQuests, "meet_team_in_magicforest"):
    ->meet
-else:
    ->generic
}

=== meet ===
<color=\#ff00ffff>Марк</color>: Вот и ты, наконец.
Как видишь, мы с Лаурой привезли сюда наше устройство. 
<color=lightblue>Лаура</color>: Оно практически готово. В этом месте полно магической энергии, так что мы должны найти способ запитать и запустить его.
    +[Смотрите, там кто-то стоит.]
        <color=\#ff00ffff>Марк</color>: Где? Хм, и действительно...
        <color=lightblue>Лаура</color>: Неужели мы попали в ловушку? Это бандит?
        ~invoke_dialogue_event_universal("meet_team_in_magicforest")
        <color=\#ff00ffff>Марк</color>: Не похож на бандита. Однако этот человек кажется мне знакомым...
        Давайте подойдем и узнаем, что ему нужно от нас.
->END
=== generic ===
Ок
->END