INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()
{contains(activeQuests, "party_gather_food"):
    ->bring_items
}
{contains(activeQuests, "party_gather_alcohol"):
    ->nextStep
-else:
    ->generic
}

=== bring_items ===
    +[Я принёс фрукты.]
        ~open_item_container("party_gather_food", 2)
            Отличная работа!
            ->validation
{has_enough_items_for_this_goal("party_gather_food", 0):
    +[Я принес пироги.]
    Отлично! Давай их сюда.
        ~invoke_dialogue_event("party_gather_pie")
        ->validation
    ->END
}
{has_enough_items_for_this_goal("party_gather_food", 1):
    +[Я принес чизкейки.]
    Отлично! Давай их сюда.
        ~invoke_dialogue_event("party_gather_cheesecake")
        ->validation
-else:
    +[Я вернусь позже.]
        Без проблем! Только не пропадай надолго, очень уж хочется расслабиться и посидеть с друзьями...
        ->END
    
}
=== nextStep ===
    Похоже, ты собрал всю еду, которую я заказывал. Отлично!
    Я подумал, и пришёл к выводу, что на вечеринке нужно расслабиться и чего-нибудь выпить.
    Не сильно крепкое, например вино отлично подойдёт. Пускай будет и белое, и красное, по паре бутылочек. Приходи, когда соберёшь!
    ->END
=== generic ===
    Уже собрал? Я ещё не освободил место в своём сундуке, приходи чуть позже.
    ->END
 
 === validation ===
 ~temp currentActive = get_activeQuestList()
 {not contains(currentActive, "party_gather_food"): //Раньше был активным, сейчас нет => выполнил
    ->nextStep
-else:
    Вижу, ещё чего-то не хватает. Буду ждать, когда соберёшь остальное!
    ->bring_items
}