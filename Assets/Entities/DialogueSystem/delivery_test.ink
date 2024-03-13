INCLUDE MainInkLibrary.ink

-> greeting

=== greeting ===

Салам алейкум.
-> general

=== general ===

~temp questSummaries = get_activeQuestList()
{not is_empty(questSummaries):
    +[По поводу моего задания...]
    -> quests
}
+[Есть для меня какая-нибудь работа?]
    ->give_quest
+[Пока]
    До побачення.
    ->END
    

=== give_quest ===
{not check_if_quest_has_been_taken("delivery_test"):
    Да, у меня есть для тебя задание. Видишь ли, начинается осень, стало уже прохладно.
    Моя одежда уже совсем износилась. Принеси мне любой одежды, какой угодно.
    Скажем, 4 штуки. Я заплачу тебе. Согласен?
        +[Берусь.]
            Отлично!
            ~add_quest("delivery_test")
            ->general
        +[Не, какая-то лажа.]
            Жаль.
            ->general
-else:
    На данный момент у меня для тебя ничего нет.
    ->general
}

=== quests ===
~temp questSummaries = get_activeQuestList()
{contains(questSummaries, "delivery_test"):
    У тебя есть что-то для меня?
    +[Я принес тебе твою одежду]
        ~open_item_container("delivery_test", 0)
        Ну-ка, ну-ка...
        Отлично! Я знал, что на тебя можно положиться. Держи награду.
        ->general
}