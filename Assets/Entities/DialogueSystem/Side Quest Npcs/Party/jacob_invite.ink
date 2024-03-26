INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "party_gather_friends"):
    ->invite
-else:
    ->generic
}

=== invite ===
{not is_goal_completed("party_gather_friends", 1):
Чего хотел? Я занят.
    +[Ваш друг Сэм приглашает вас на вечеринку.]
        Сэм? Вот так дела... Я думал, он уже сбежал в другую страну...
        Что ж, приду, нужно встретиться и поговорить. Придётся отложить работу на время.
        ~invoke_dialogue_event("party_gather_friends_jacob")
        Спасибо, что сказал. Этот Сэм, кажется и денег мне должен...
        ->END
-else:
Спасибо, что сказал о том, что Сэм собирает людей. У меня к нему серьёзный разговор.
->END
}
=== generic ===
{check_if_quest_has_been_taken("party_gather_friends"):
    Чего тебе?
        +[До свидания.]
            Иди-иди.
            ->END
-else:
    Спасибо, что сказал о том, что Сэм собирает людей. У меня к нему серьёзный разговор.
    ->END
}