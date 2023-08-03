INCLUDE MainInkLibrary.ink

->greeting

=== greeting ===

Привет, я ЖЭБА
-> general


=== general ===
LIST myList = amogus
~ myList = get_active_quests_for_this_npc()
~debug_log(myList)

+[Дай квест 1]
    Даю
    ~add_quest("zheba_cactus")
    ->general
+[Дай квест 2]
    Даю
    ~add_quest("zheba_strela")
    ->general
+[Пока]
    Ну пока.
    -> END