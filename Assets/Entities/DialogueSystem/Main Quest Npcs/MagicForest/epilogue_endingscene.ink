INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList_universal()

{contains (activeQuests, "activate_device"):
->activate
}

=== activate ===
\*Вы видите, как ваш отец активирует устройство. Всё озаряется ярчайшим блеском, и вы невольно зажмуриваетесь...*
~invoke_dialogue_event("activate_device")
->END