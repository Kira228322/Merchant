INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "bribe_vice_headman"):
    {is_goal_completed("bribe_vice_headman", 0):
        А, это снова ты. Что скажешь о нашей сделке?
        ->bribe
    -else:
        ->firstGreeting
    }
-else:
    ->generic
}

=== firstGreeting ===
Человек! Все в нашей деревне говорят о тебе.
Ходят слухи, будто бы ты всем помогаешь просто так. От меня ты чего хотел?
+[Говорят, ты друг старейшины Глорга.]
        Какой он мне друг, дурак старый! Раньше был нормальной жабой, а сейчас совсем оскотинился.
            ++[Зачем же ты его поддерживаешь?]
                Ну потому что лучше с ним, чем ни с кем. Хотя бы бандиты не зверствуют, и то хорошо. А вообще я бы просто уехал куда-нибудь в нормальное место, да денег нет.
                    +++[Может, я могу помочь тебе с этим? (200 золота)]
                        ~invoke_dialogue_event("vice_headman_met")
                        Может, и можешь... Что ты имеешь в виду, человек?
                        ->bribe
                    +++[А что ты скажешь, если Флурп станет старейшиной?]
                        Флурп? Он слишком молод для такой ноши. К тому же, он слишком хитрый, как будто что-то скрывает...
                            ++++[У меня есть к тебе предложение. (200 золота)]
                                ~invoke_dialogue_event("vice_headman_met")
                                Что за предложение, человек?
                                ->bribe
=== bribe ===
    Ну же, не тяни головастика за хвост!
    {has_money(200):
    +[*Отдать 200 золота* Держи, это деньги для твоего переезда.]
        Вот это другое дело! Спасибо за поддержку. Этого как раз хватит, чтобы уехать в другой регион и купить себе землянку.
        ~invoke_dialogue_event("vice_headman_bribed")
        Не знаю, с чего ты помогаешь мне, но, видимо, не все люди такие уж плохие...
        Спасибо и прощай!
        ->END
    }
    +[Я вернусь позже.]
        Что это было, человек? Зачем ты делаешь какие-то намёки, а затем уходишь?
        ->END
    
                        

=== generic ===
Ну, чего надо?
    +[Ничего, я уже ухожу.]
        Уходи, человек. Ты сильно мешаешь.
        ->END