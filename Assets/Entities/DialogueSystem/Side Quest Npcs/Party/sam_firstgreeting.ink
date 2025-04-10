INCLUDE ../../MainInkLibrary.ink

{not check_if_quest_has_been_taken("party_gather_food"):
    ->give_quest
-else:
    ->generic
}

=== give_quest ===
Привет! Сегодня отличный день, не так ли?
    +[Похоже, у тебя хорошее настроение.]
        А то!
        Жаль только, с друзьями мы давно не собирались. Я подумываю закатить хорошую вечеринку, как в старые добрые времена.
        Ты не поможешь? По тебе видно, что путешествовать ты привык. Всего-то нужно поездить по королевству и собрать вкусной еды.
        Знаешь, хочется чего-то экзотического. У нас в северных землях редко найдешь свежие фрукты. А ещё мы очень любим пироги и чизкейки.
        В общем собери чего-нибудь такого. Разумеется, я отплачу тебе. Ну, что скажешь?
            ++[Хорошо, я не против помочь.]
                Замечательно! Хорошо, я рассчитываю на тебя.
                ~add_quest("party_gather_food")
                С нетерпением буду ждать тебя!
                ->END
            ++[Как-нибудь в другой раз.]
                Жаль, ну ладно. Приходи, если передумаешь!
                ->END

=== generic ===       
~temp activeQuests = get_activeQuestList()
{contains(activeQuests, "party_gather_food"):
    Ну, как проходит подготовка?
    ->bring_items
-else:
    Спасибо за помощь! Приходи ко мне позже, я подумаю, что делать дальше.
    ->END
}

=== bring_items ===

{has_enough_items_for_this_goal("party_gather_food", 0) && not is_goal_completed("party_gather_food", 0):
    +[Я принес пироги.]
    Отлично! Давай их сюда.
        ~invoke_dialogue_event("party_gather_pie")
        ->validation
    ->END
}
{has_enough_items_for_this_goal("party_gather_food", 1) && not is_goal_completed("party_gather_food", 1):
    +[Я принес чизкейки.]
    Отлично! Давай их сюда.
        ~invoke_dialogue_event("party_gather_cheesecake")
        ->validation
-else:
    +[Я вернусь позже.]
        Без проблем! Только не пропадай надолго, очень уж хочется расслабиться и посидеть с друзьями...
        ->END
    
}
{not is_goal_completed("party_gather_food", 2):
    +[Я принёс фрукты.]
        ~open_item_container("party_gather_food", 2)
            Отличная работа!
            ->validation
}

 === validation ===
 ~temp currentActive = get_activeQuestList()
 {not contains(currentActive, "party_gather_food"): //Раньше был активным, сейчас нет => выполнил
    ->nextStep
-else:
    Вижу, ещё чего-то не хватает. Буду ждать, когда соберёшь остальное!
    ->bring_items
}

=== nextStep ===
    Похоже, ты собрал всю еду, которую я заказывал. Отлично!
    Я подумал, и пришёл к выводу, что на вечеринке нужно расслабиться и чего-нибудь выпить.
    Не сильно крепкое, например вино отлично подойдёт. Пускай будет и белое, и красное, по паре бутылочек. Приходи, когда соберёшь!
    ->END
