INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_south_swords"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_south_swords"):
    {has_enough_items("questgiver_south_swords"):
        +[Я принёс тебе железные мечи.]
            Отлично! Осталось только дать нашим бравым ребятам эти мечи, и никакие бандиты нам не страшны! Молодец, отличная работа! 
            ->exotic_reward
    }
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_south_swords")
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
 - contains(randomQuest, "questgiver_south_swords"):
    ~add_quest("questgiver_south_swords")
        У нашей деревни сейчас некоторые проблемы. В последнее время участились нападения бандитов. Они грабят торговцев и вымогают деньги с простых людей
        Непонятно, куда смотрит стража... В общем, я договорился с крепкими мужиками, они согласны патрулировать окрестности.
        Не хватает только оружия для них. Пожалуйста, раздобудь где-нибудь железные мечи и притащи мне. Ты сослужишь очень хорошую службу для деревни. Разумеется, я также заплачу тебе деньгами.
        ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END