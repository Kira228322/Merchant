INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_desert_exotic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_desert_domestic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_desert_categories"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_desert_exotic_items"):
    {has_enough_items("questgiver_desert_exotic_items"):
        +[Я принёс тебе экзотические предметы.]
            Отлично! Из этого алоэ получится отличная мазь - как раз то, что нужно в пустыне!
            ->exotic_reward
    }
}
{contains(activeQuests, "questgiver_desert_domestic_items"): 
    {has_enough_items("questgiver_desert_domestic_items"):
        +[Я принес тебе семена древомрака.]
            Отлично, спасибо тебе! Надеюсь, они приживутся в наших условиях...
            ->domestic_reward
    }
}
{contains(activeQuests, "questgiver_desert_categories"):
    +[Я принес тебе предметы определенной категории.]
        ~open_item_container("questgiver_desert_categories", 0)
            Ну-ка, показывай, что у тебя...
            ->category_reward
        
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_desert_exotic_items")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event("questgiver_desert_domestic_items")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== category_reward ===
Отлично, это то, что нужно! Вот, возьми свою награду. С тобой приятно иметь дело!
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
 - contains(randomQuest, "questgiver_desert_exotic_items"):

        Пустыня - суровое место. Кроме ужасной жары и отсутствия цивилизации, там обитают опасные монстры. Людям, которые ходят в пустыню, нужна целебная мазь.
        К сожалению, мои запасы алоэ, основного ингредиента мази, подходят к концу. Ты не мог бы привезти ещё?
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_desert_exotic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
 - contains(randomQuest, "questgiver_desert_domestic_items"):

        Я хотела попробовать посадить такое растение, как древомрак. В пустыне редко что-то выращивают, но я читала в книгах, что древомрак довольно неприхотлив.
        Мне всего лишь нужно несколько семян. Разумеется, я заплачу.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_desert_domestic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
 - contains(randomQuest, "questgiver_desert_categories"):

        Недавно один караванщик попал в беду. В пустыне на него напал монстр и повредил ему глаза. Мы залечили его как могли, и сейчас он восстанавливается.
        Для хорошего выздоровления нужна правильная еда - например богатая витаминами и фосфором рыба. К сожалению, рыбы в пустыне не найти.
        Прошу тебя, привези любой рыбы из соседних регионов. Только не медли, потому как на кону здоровье этого караванщика! Разумеется, мы тебе заплатим.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_desert_categories")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END