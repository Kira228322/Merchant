INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "bring_fish_relic_to_laura"):
    ->bring_relic
-else:
    ->generic
}

=== bring_relic ===

->END

=== generic ===

->END