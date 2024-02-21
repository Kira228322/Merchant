INCLUDE ../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "questgiver_middle_exotic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_middle_domestic_items"):
    ->active_quest
}

{contains(activeQuests, "questgiver_middle_categories"):
    ->active_quest
}

    ->check_cooldown


=== active_quest ===
{contains(activeQuests, "questgiver_middle_exotic_items"):
    {has_enough_items("questgiver_middle_exotic_items"):
        +[Я принёс тебе экзотические предметы.]
            Отлично! Какая ароматная ветчина! Ты хорошо послужил мне, простолюдин.
            ->exotic_reward
    }
}
{contains(activeQuests, "questgiver_middle_domestic_items"): 
    {has_enough_items("questgiver_middle_domestic_items"):
        +[Я принес тебе местные предметы.]
            Ах да, я уже и забыл, как хотел получить эти доски. Что ж, попрошу рабочих начать постройку таверны.
            ->domestic_reward
    }
}
{contains(activeQuests, "questgiver_middle_categories"):
    +[Я принес тебе предметы определенной категории.]
        ~open_item_container("questgiver_middle_categories", 0)
            Ну-ка, показывай, что у тебя...
            ->category_reward
        
-else:
    +[Я вернусь позже.]
        Пока.
        ->END
}

=== exotic_reward ===
~invoke_dialogue_event("questgiver_middle_exotic_items")
Вот, возьми свою награду. С тобой приятно иметь дело!
->END
=== domestic_reward ===
~invoke_dialogue_event("questgiver_middle_domestic_items")
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
 - contains(randomQuest, "questgiver_middle_exotic_items"):

        Слушай внимательно. Каждую неделю мы устраиваем пир в нашей компании. Скоро - моя очередь. Я хочу, чтобы все мои друзья меня похвалили и увидели, насколько я хорош.
        Для этого ты привезёшь мне свежайшую ветчину из южных земель. Только попробуй съесть её по дороге!
        Ах да, и не вздумай опоздать! Иначе ты нанесешь урон моей чести и выставишь меня дураком.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_middle_exotic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Тогда уйди с глаз моих, простолюдин...
                ->END
 - contains(randomQuest, "questgiver_middle_domestic_items"):

        Я желаю, чтобы все в городе любили и почитали меня, так что я решил сделать какое-нибудь доброе дело, например построить новую таверну.
        Рабочих я уже нанял, но им не хватает материалов. Привези мне много деревянных досок, и тогда я подумаю, какой награды тебя удостоить!
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_middle_domestic_items")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Тогда уйди с глаз моих, простолюдин...
                ->END
 - contains(randomQuest, "questgiver_middle_categories"):

        Я понимаю, что ты живёшь где-то в бедных землях, но ты должен был слышать о моём великом экзотическом саду! Там растут всяческие редкие растения из других земель.
        Я хотел расширить этот сад, высадив там кактусы, привезённые из пустыни. Так что привези мне всякие разные кактусы, и я щедро тебя награжу, простолюдин.
        Ну как, берёшься?
            +[Хорошо, я согласен.]
                ~add_quest("questgiver_middle_categories")
                Отлично!
                ->END
            +[Нет, в другой раз.]
                Тогда уйди с глаз моих, простолюдин...
                ->END
}


=== generic ===
Привет! На данный момент для тебя нет работы. Приходи позже.
->END