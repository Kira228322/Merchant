INCLUDE MainInkLibrary.ink
->greeting

=== greeting ===

Привет, я ЖЭБА
-> general


=== general ===

~temp questSummaries = get_activeQuestList()
{not is_empty(questSummaries):
    +[По поводу моего задания...]
        -> quests
}
+[Есть для меня какая-нибудь работа?]
    -> give_random_quest
+[Пока]
    Ну пока.
    -> END
    
    
=== give_random_quest ===
{is_questgiver_ready_to_give_quest():
    ~temp questSummary = get_random_questparams_of_questgiver()
    {questSummary == "zheba_cactus":
        -> give_quest1
    }
    {questSummary == "zheba_strela":
        -> give_quest2
    }
-else:
Нет, в данный момент у меня нет новых заданий для тебя.
-> general
}

=== give_quest1 ===
Да, как раз для тебя появилась работка. Видишь ли, я собираюсь варить кактусовый самогон.
Поэтому мне нужно несколько кактусов. Смотри, какие попало кактусы не подойдут.
Нужны только кактусы высшего качества! У нас на родине их называют "Крутецкие". Это такой особый сорт.
~add_quest("zheba_cactus")
-> general

=== give_quest2 ===
Да, у меня есть для тебя важное задание. На самом деле, я не всегда была жабой. 
Давным-давно я была прекрасной принцессой, но ведьма наложила на меня проклятие и заставила проживать остаток жизни в качестве жабы.
Проклятие может быть снято, если меня поцелует принц. 
Короче, у меня есть план: я буду стрелять из лука по проезжающим мимо принцам. 
Возможно кто-то из них захочет разобраться, в чем тут дело и я смогу заставить его поцеловать меня.
Короче, мне нужно несколько стрел.
~add_quest("zheba_strela")
-> general

=== quests ===
~temp questSummaries = get_activeQuestList()
#никогда не empty потому что это проверяется перед попаданием в этот узел
{contains(questSummaries, "zheba_cactus"):
    {has_enough_items("zheba_cactus"):
    +[Я принес тебе твои кактусы]
        ~invoke_dialogue_event("zheba_cactus_complete")
        Отлично! Я знала, что на тебя можно положиться. Сегодня устрою великую попойку!
        ->general
    - else:
    +[Я всё ещё в процессе поиска твоих кактусов.]
        Ничего страшного, время ещё есть.
        ->quests
    }
}
{contains(questSummaries, "zheba_strela"):
    {has_enough_items("zheba_strela"):
    +[Я принес тебе твои стрелы]
        ~invoke_dialogue_event("zheba_strela_complete")
        Отлично! Теперь я смогу очаровать принцев! Вот твоя награда.
        ->general
    - else:
    +[Я всё ещё в процессе поиска твоих стрел.]
        Ничего страшного, время ещё есть.
        ->quests
    }
}
+[Поговорим о чём-нибудь другом.]
    ->general


