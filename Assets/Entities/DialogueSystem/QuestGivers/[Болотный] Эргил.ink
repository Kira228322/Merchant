INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_swamp_exotic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_swamp_domestic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_swamp_categories"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_swamp_exotic_items"):
    {has_enough_items("questgiver_swamp_exotic_items"):
        +[Я принёс тебе экзотические предметы.]
            Ого, магические свечи! Ты погляди, как они хорошо светят! Теперь никакой туман нам не помешает!
            ->exotic_reward
    }
}
{contains(activeQuests, "questgiver_swamp_domestic_items"): 
    {has_enough_items("questgiver_swamp_domestic_items"):
        +[Я принес тебе местные предметы.]
            А, мясо монстра для экспериментов! Отлично, прямо сейчас и пущу его в ход! Замечательно!
            ->domestic_reward
    }
}
{contains(activeQuests, "questgiver_swamp_categories"):
    +[Я принес тебе предметы определенной категории.]
        ~open_item_container("questgiver_swamp_categories", 0)
            Ну-ка, показывай, что у тебя...
            ->category_reward
        
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_swamp_exotic_items")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event("questgiver_swamp_domestic_items")
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
            Без проблем. Только учти, что это задание может взять кто-то другой!
            ->END
-else:
    ->generic
}
=== give_quest ===
~temp randomQuest = get_random_questparams_of_questgiver()

{
 - contains(randomQuest, "questgiver_swamp_exotic_items"):
        Это очень важное задание. В последнее время на болотах по вечерам возникает очень густой туман. Через него очень плохо видно, а в тумане любят скрываться бандиты или монстры.
        Чтобы разогнать туман, мы используем магические свечи. Они светят слабо, но далеко, поэтому это то, что нужно.
        Однако запас таких свечей у нас на исходе, так что хочу тебя попросить купить и привезти ещё.
        Разумеется, чем раньше ты справишься, тем лучше.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_swamp_exotic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END

 - contains(randomQuest, "questgiver_swamp_domestic_items"):
        На болотах часто проводятся магические эксперименты. Для них нам нужны подходящие материалы. Обычно мы используем мясо гнильщиков, потому что оно хорошо подвергается воздействию магии.
        Пожалуйста, принеси несколько кусков такого мяса. Думаю, ты легко найдешь его на болотах.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_swamp_domestic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
        
 - contains(randomQuest, "questgiver_swamp_categories"):
        У меня много друзей на болотах, но жизнь здесь довольно скучна. Я хотел сделать им сюрприз и угостить их редкой едой.
        Будь добр, принеси любую еду из северных земель, я не знаю что у них там. Варенье или что-то вроде того. Разумеется, я хорошо отплачу. 
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_swamp_categories")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
}



=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END