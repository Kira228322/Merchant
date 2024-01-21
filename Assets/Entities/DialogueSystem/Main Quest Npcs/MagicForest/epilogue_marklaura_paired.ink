INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "meet_team_in_magicforest"):
    ->meet
-else:
    ->generic
}

=== meet ===

=== generic ===