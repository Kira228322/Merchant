INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "party_gather_friends"):
    ->invite
-else:
    ->generic
}

=== invite ===
{not is_goal_completed("party_gather_friends", 0):
Привет, малыш. Тебе что-то нужно от меня?
    +[Ваш друг Сэм приглашает вас на вечеринку.]
        Сэмми? Ничего себе! Столько времени от него ничего не было слышно, и вдруг решил вспомнить!
        Что ж, приду. Надеюсь, там будет приятные собеседники, а то мне совсем уж скучно здесь.
        ~invoke_dialogue_event("party_gather_friends_dolores")
        Он же живёт там, где и раньше? Ладно, пойду собираться. Спасибо, что передал, мальчик!
        ->END
-else:
А ты забавный. Приходи тоже на вечеринку, познакомимся поближе...
Ладно, иди зови остальных. Сэмми ведь не одну меня позвал? Иначе мне с ним будет слишком скучно...
->END
}
=== generic ===
{check_if_quest_has_been_taken("party_gather_friends"):
    Тебе что-то нужно, сладкий?
        +[Извините, я пойду.]
            Пока!
            ->END
-else:
    Да-да, я уже собираюсь ехать.
    ->END
}