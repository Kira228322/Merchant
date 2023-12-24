INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList_universal()

{contains(activeQuests, "investigate_hidden_fish_evidence"):
    ->investigation
-else:
    ->generic
}

=== investigation ===
Как и говорил Флурп, перед собой вы видите место странного ритуала.
Возле странного магического камня разбросаны различные вещи.
Вы видите кинжал, воткнутый в землю, бутылки и банки с неизвестным содержимым, а также мешок с непонятными предметами внутри.
Вам не ясно, для чего могли быть использованы все эти вещи, но вас пробирает дрожь...
~invoke_dialogue_event_universal("investigate_hidden_fish_evidence")
~place_item("Доказательства ритуала", 1, 0)
Вы собираете пару предметов и уходите.
->END


=== generic ===
Здесь больше ничего не осталось, что можно было бы собрать. Вы решаете поскорее уйти из этого неспокойного места...
->END