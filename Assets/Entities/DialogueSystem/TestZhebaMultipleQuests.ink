INCLUDE MainInkLibrary.ink
->greeting

=== greeting ===

Привет, я ЖЭБА
-> general


=== general ===

~temp questSummaries = get_activeQuestList()
~debug_log(contains(questSummaries, "zheba_cactus"))
~debug_log(contains(questSummaries, "zheba_strela"))

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