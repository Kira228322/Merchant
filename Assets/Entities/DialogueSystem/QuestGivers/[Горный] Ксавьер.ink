INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_mountain_exotic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_mountain_domestic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_mountain_categories"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_mountain_exotic_items"):
    {has_enough_items("questgiver_mountain_exotic_items"):
        +[Я принёс тебе экзотические предметы.]
            Замечательно, это то, что мне было нужно! Уверен, моей жене понравятся такие духи!
            ->exotic_reward
    }
}
{contains(activeQuests, "questgiver_mountain_domestic_items"): 
    {has_enough_items("questgiver_mountain_domestic_items"):
        +[Я принес тебе местные предметы.]
            А, ты про горный чай? Да, чувствую, собран с самых лучших плантаций высоко в горах. Спасибо!
            ->domestic_reward
    }
}
{contains(activeQuests, "questgiver_mountain_categories"):
    +[Я принес тебе предметы определенной категории.]
        ~open_item_container("questgiver_mountain_categories", 0)
            Ну-ка, показывай, что у тебя...
            ->category_reward
        
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_mountain_exotic_items")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event("questgiver_mountain_domestic_items")
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
 - contains(randomQuest, "questgiver_mountain_exotic_items"):

        У меня скоро годовщина свадьбы, и я хотел сделать своей жене особый подарок.
        Я думаю подарить ей какие-нибудь хорошие духи, но у нас в горах редко их найдёшь.
        Ты не мог бы привезти мне их откуда-нибудь? Разумеется, я отплачу.
        И пожалуйста, успей за одну неделю! Не хочу разочаровывать жену отсутствием подарка.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_mountain_exotic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
                
 - contains(randomQuest, "questgiver_mountain_domestic_items"):

        Ко мне скоро приедут гости из далёких земель. Они никогда не бывали у нас в горах, так что я хотел дать им попробовать нашего знаменитого зелёного чая. Пожалуйста, поищи для меня хороших листьев! Я достойно награжу тебя.
        Только, прошу, успей до приезда моих гостей.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_mountain_domestic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
 - contains(randomQuest, "questgiver_mountain_categories"):
    
        Ты знаешь, я живу в большом особняке, но в последнее время он какой-то неуютный. Я думал, его можно украсить различными трофеями. 
        Однако охотиться я не очень люблю, поэтому хотел попросить тебя, чтобы ты принёс разные трофеи с монстров. Не знаю, где ты их достанешь, но, разумеется, я отплачу!
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_mountain_categories")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END