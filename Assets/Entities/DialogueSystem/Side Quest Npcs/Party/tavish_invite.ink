INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "party_gather_friends"):
    ->invite
-else:
    ->generic
}

=== invite ===
{not is_goal_completed("party_gather_friends", 2):
\*Тавишу трудно стоять на ногах, в его руке бутылка*
Ик! Чего надо, расплывчатое пятно?
    +[Ваш друг Сэм приглашает вас на вечеринку.]
        Вече...ринку? Ик! Отлично, у меня как раз деньги закончились!
        Где вечеринка - там и выпивка, ха-ха-ха!
        ~invoke_dialogue_event("party_gather_friends_tavish")
        Правда, не помню, что за Сэм... Я только одного Сэма знаю, вот он! *Тавиш трясёт бутылкой*
        Ну и ладно, ик! Главное - хорошо провести время, так ведь, дружище?
        ->END
-else:
    Ик! Вечеринка, я иду к тебе!
->END
}
=== generic ===
{check_if_quest_has_been_taken("party_gather_friends"):
    Уважаемый, у вас не будет монетки?
        +[Извините, нет.]
            Эх, перевелись добрые люди... Ик!
            ->END
-else:
    Ик! Вечеринка, я иду к тебе!
    ->END
}