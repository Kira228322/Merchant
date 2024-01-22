INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "meet_team_in_magicforest"):
    ->meet
-else:
    ->generic
}

=== meet ===
<color=green>Марк:</color>Как здорово, что все мы здесь сегодня собрались
<color=lightblue>Лаура:</color>Как здорово, что все мы здесь сегодня собрались
->END
=== generic ===
Ок
->END