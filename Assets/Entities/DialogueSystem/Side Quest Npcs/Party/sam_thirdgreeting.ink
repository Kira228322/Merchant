INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()
{contains(activeQuests, "party_gather_alcohol"):
    ->bring_items
}
{contains(activeQuests, "party_gather_friends"):
    ->nextStep
-else:
    ->generic
}

=== bring_items ===

{has_enough_items_for_this_goal("party_gather_alcohol", 0):
    +[Я принес красное вино.]
    Отлично! Давай мне, я уберу в безопасное место.
        ~invoke_dialogue_event("party_gather_redwine")
        ->validation
    ->END
}
{has_enough_items_for_this_goal("party_gather_alcohol", 1):
    +[Я принес белое вино.]
    Отлично! Давай мне, я уберу в безопасное место.
        ~invoke_dialogue_event("party_gather_whitewine")
        ->validation
}
    +[Я вернусь позже.]
        Без проблем! Только не пропадай надолго, очень уж хочется расслабиться и посидеть с друзьями...
        ->END
    
=== nextStep ===
    Отлично, с выпивкой мы тоже разобрались. Похоже, всё готово к вечеринке!
    TODO откуда друзья
    Осталось только позвать друзей. Я думал пригласить троих: Долорес из деревни TODO, Джейкоба из TODO и Тавиша из TODO.
    Мы все очень хорошо дружим, но уже давно вместе не собирались...
    В общем, пожалуйста, съезди и позови их всех. Надеюсь, они согласятся прийти!
    ->END
=== generic ===
    Осталось только позвать друзей!
    ->END
 
 === validation ===
 ~temp currentActive = get_activeQuestList()
 {not contains(currentActive, "party_gather_alcohol"): //Раньше был активным, сейчас нет => выполнил
    ->nextStep
-else:
    Вижу, не хватает другого вида вина. Я буду ждать!
    ->bring_items
}