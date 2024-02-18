INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, Экзотический):
    ->active_quest
}

{contains(activeQuests, Домашний):
    ->active_quest
}

{contains(activeQuests, Категория):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, Экзотический) && has_enough_items(Экзотический):
    +[Я принёс тебе экзотические предметы.]
        Замечательно, ты отлично справился!
        ->exotic_reward
}
{contains(activeQuests, Домашний) && has_enough_items(Домашний):
    +[Я принес тебе местные предметы.]
        Замечательно, ты отлично справился!
        ->domestic_reward
}
{contains(activeQuests, Категория):
    +[Я принес тебе предметы определенной категории.]
        ~open_item_container(Категория, 0)
            Ну-ка, показывай, что у тебя...
            ->category_reward
        
-else:
    +[Я вернусь позже.]
}

=== exotic_reward ===
~invoke_dialogue_event(Экзотический)
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event(Домашний)
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== category_reward ===
Вот, возьми свою награду. С тобой приятно иметь дело!
->END

=== check_cooldown ===
{is_questgiver_ready_to_give_quest():
    Привет! У меня есть для тебя задание, не желаешь взглянуть?
        +[Хорошо, покажи, что у тебя есть.]
            ->give_quest
        +[В другой раз.]
            Без проблем. Только учти, что это задание возьмёт кто-то другой!
            ->END
-else:
    ->generic
}
=== give_quest ===
~temp randomQuest = get_random_questparams_of_questgiver()

{
 - contains(randomQuest, Экзотический):
    ~add_quest(Экзотический)
        ЭкзотическийОписание
        ->END
 - contains(randomQuest, Домашний):
    ~add_quest(Домашний)
        ДомашнийОписание
        ->END
 - contains(randomQuest, Категория):
    ~add_quest(Категория)
        КатегорияОписание
        ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END