INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "meet_team_in_magicforest"):
    ->meet
-else:
    ->generic
}

=== meet ===
<color=green>Марк</color>: Вот и ты, наконец.
Как видишь, мы с Лаурой привезли сюда наше устройство. 
<color=lightblue>Лаура</color>: Оно практически готово. В этом месте полно магической энергии, так что мы должны найти способ запитать и запустить его.
    +[Смотрите, там кто-то стоит.]
        <color=green>Марк</color>: Где? Хм, и действительно...
        <color=lightblue>Лаура</color>: Неужели мы попали в ловушку? Это бандит?
        <color=green>Марк</color>: Не похож на бандита. Однако этот человек кажется мне знакомым...
->END
=== generic ===
Ок
->END