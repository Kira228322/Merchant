INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_middle_shields"):
    ->active_quest
}

{contains(activeQuests, "questgiver_middle_arrows"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_middle_shields"):
    {has_enough_items("questgiver_middle_shields"):
        +[Я принёс тебе железные щиты по заказу.]
            Отлично! Будь уверен, они послужат на славу нашей гильдии и спасут множество жизней!
            ->exotic_reward
    }
}
{contains(activeQuests, "questgiver_middle_arrows"): 
    {has_enough_items("questgiver_middle_arrows"):
        +[Я принес тебе стрелы по заказу.]
            Отлично, наши запасы как раз подходили к концу. Спасибо тебе!
            ->domestic_reward
    }
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_middle_shields")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event("questgiver_middle_arrows")
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
 - contains(randomQuest, "questgiver_middle_shields"):

        Наша Гильдия бойцов проводит набор всех желающих. Разумеется, тебе не предлагаю, ведь ты торговец, но я хотел попросить тебя о другом.
        Для новобранцев понадобится экипировка. Мечи у нас есть, но не хватает железных щитов, а боец без защиты - всё равно что лист при урагане.
        В общем, добудь несколько щитов и привези нам, а мы тебя хорошо вознаградим.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_middle_shields")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
 - contains(randomQuest, "questgiver_middle_arrows"):

        Наша Гильдия бойцов сейчас тренирует новичков. К сожалению, не все из них ответственные, и некоторые теряют экипировку. 
        Особенно тяжело сейчас со стрелами. Будь добр, раздобудь где нибудь несколько колчанов стрел, и мы вознаградим тебя. 
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_middle_arrows")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Жаль, ну что ж, твоё дело.
                ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END