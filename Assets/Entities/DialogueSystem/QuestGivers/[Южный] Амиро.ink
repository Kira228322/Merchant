INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_south_exotic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_south_domestic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_south_categories"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_south_exotic_items"):
    {has_enough_items("questgiver_south_exotic_items"):
        +[Я принёс тебе экзотические предметы.]
            Замечательно, ты отлично справился! Мои друзья порадуются вкуснейшему пиву!
            ->exotic_reward
    }
}
{contains(activeQuests, "questgiver_south_domestic_items"): 
    {has_enough_items("questgiver_south_domestic_items"):
        +[Я принес тебе местные предметы.]
            Замечательно, ты отлично справился! Ух, ягодный пирог выйдет на славу!
            ->domestic_reward
    }
}
{contains(activeQuests, "questgiver_south_categories"):
    +[Я принес тебе предметы определенной категории.]
        ~open_item_container("questgiver_south_categories", 0)
            Ну-ка, показывай, что у тебя...
            ->category_reward
        
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_south_exotic_items")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event("questgiver_south_domestic_items")
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
 - contains(randomQuest, "questgiver_south_exotic_items"):
    ~add_quest("questgiver_south_exotic_items")
        Ко мне недавно приехали мои старые друзья. Они какое-то время будут жить со мной, и я хотел показать им все красоты южного региона.
        Мы также хотели отдохнуть где-то на природе шумной компанией.
        Хочу тебя попросить, чтобы ты принёс нам пива. Понимаю, что пиво у нас здесь ценится не слишком сильно, но надеюсь на твои навыки путешественника!
        И пожалуйста, успей до начала вечеринки.
        ->END
 - contains(randomQuest, "questgiver_south_domestic_items"):
    ~add_quest("questgiver_south_domestic_items")
        Мои друзья попросили меня показать мои навыки кулинара. Я люблю готовить, но в этот раз решил приготовить кое-что особенное - большой ягодный пирог! 
        Мне понадобится куча красных ягод, соберешь их для меня? Только, прошу тебя, успей за неделю, чтобы не разочаровать моих друзей!
        ->END
 - contains(randomQuest, "questgiver_south_categories"):
    ~add_quest("questgiver_south_categories")
        В последнее время я устал от однообразия и скукоты. Хотел приготовить какое-нибудь экзотическое блюдо, например, что-нибудь восточное.
        Понадобятся любые восточные специи, которые только найдёшь. Принеси их, пожалуйста!
        И, будь добр, уложись в одну неделю - иначе мои другие ингредиенты испортятся!
        ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END