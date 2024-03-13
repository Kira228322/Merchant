INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_sea_exotic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_sea_domestic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_sea_categories"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_sea_exotic_items"):
    {has_enough_items("questgiver_sea_exotic_items"):
        +[Я принёс тебе экзотические предметы.]
            Отлично! Это серебряное ожерелье - то что нужно. Оно очень красивое, спасибо! 
            ->exotic_reward
    }
}
{contains(activeQuests, "questgiver_sea_domestic_items"): 
    {has_enough_items("questgiver_sea_domestic_items"):
        +[Я принес тебе местные предметы.]
            Ах да, непромокаемые плащи! Теперь мы готовы отправляться в путешествие. Спасибо!
            ->domestic_reward
    }
}
{contains(activeQuests, "questgiver_sea_categories"):
    +[Я принес тебе предметы определенной категории.]
        ~open_item_container("questgiver_sea_categories", 0)
            Ну-ка, показывай, что у тебя...
            ->category_reward
        
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_sea_exotic_items")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event("questgiver_sea_domestic_items")
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
 - contains(randomQuest, "questgiver_sea_exotic_items"):

        Задание довольно простое - я хотел приобрести серебряное ожерелье. Мне всегда нравилось, как серебро своим тусклым блеском напоминает о морской воде...
        Что-то меня потянуло на философские изречения, извини. В общем, просто принеси ожерелье, а я тебе отплачу.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_sea_exotic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
 - contains(randomQuest, "questgiver_sea_domestic_items"):

        Скоро мы с другом отправляемся в путешествие на яхте. У нас уже всё готово, но не хватает плащей для защиты от непогоды. Сможешь доставить их мне?
        Только учти, что нужно успеть до того, как мы отплывём.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_sea_domestic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
 - contains(randomQuest, "questgiver_sea_categories"):
    
        Для моряка очень важно потреблять достаточно витаминов и полезных веществ, а не то заболеешь! Больше всего витаминов в свежих южных ягодах и фруктах, а также они очень вкусные.
        Пожалуйста, принеси лобых южных ягод или фруктов, а я в долгу не останусь.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_sea_categories")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END