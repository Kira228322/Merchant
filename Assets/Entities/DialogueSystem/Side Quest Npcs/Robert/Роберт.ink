INCLUDE ../../MainInkLibrary.ink

-> greeting

=== greeting ===

Привет!

~temp questSummaries = get_activeQuestList()
{contains(questSummaries, "crafting_lesson_make_belt"):
-> making_belt
}
{contains(questSummaries, "crafting_lesson_make_planks"):
-> making_planks
}
{not check_if_quest_has_been_taken("crafting_lesson_make_belt"):
    +[Чем занимаешься?]
        -> quest_offering  
}
+[Мне пора идти.]
    ->END
    
=== crafting_lesson_start ===
Отлично!
    ~place_item("Кожа", 1, 0)
    ~place_item("Веревка", 1, 0)
    ~place_item("Рецепт (пояс)", 1, 0)
    ~add_quest("crafting_lesson_make_belt")
Смотри, всё просто. Возьми вот этот кусок кожи и веревку. Попробуй сделать из них пояс. Можешь взять вот этот рецепт, если нужна пошаговая инструкция.
    ->END


=== making_belt ===
Ну как там успехи с поясом?
    {has_enough_items("crafting_lesson_make_belt"):
        +[Вот, погляди. (Отдать 1х Пояс)]
            ~invoke_dialogue_event("crafting_lesson_make_belt")
            Отличная работа! У тебя вышел замечательный пояс.
            Вот тебе немного другое задание: нужно распилить бревно на доски.
            ~place_item("Бревно", 3, 0)
            ~place_item("Рецепт (деревянная доска)", 1, 0)
            ~add_quest("crafting_lesson_make_planks")
            Всё ещё проще, но теперь тебе нужен инструмент. На моём верстаке есть пила, можешь воспользоваться ею.
            ->END
    }
    +[Я пока работаю.]
        ->END


=== making_planks ===
Ну как, получилось распилить бревно?
    {has_enough_items("crafting_lesson_make_planks"):
        +[Вот, погляди. (Отдать 3х Деревянная доска)]
            ~invoke_dialogue_event("crafting_lesson_make_planks")
            Замечательно, спасибо тебе за помощь! Доски мне пригодятся, а вот пояс можешь оставить себе.
                ~place_item("Пояс", 1, 0)
            Постоянно практикуйся и развивай своё мастерство. Создавать предметы умеют не все, но это очень полезное дело.
            Кто знает, может быть, когда нибудь вспомнишь меня добрым словом!
            ->END
    }
    +[Я пока работаю.]
        ->END

=== quest_offering ===
Решил немного поработать на улице. Свежий воздух полезен для здоровья!
Я смотрю, руки у тебя крепкие. Не хочешь немного помочь мне? Я могу поучить тебя своему ремеслу.
    +[Давай.]
        -> crafting_lesson_start
    }
    +[Как-нибудь в другой раз.]
        ->END
    
