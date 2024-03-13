INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "interlude_bring_materials_1"):
    ->bring_materials
-else:
    ->generic
}

=== bring_materials ===
Ну как, удалось ли собрать что-нибудь?
    {has_enough_items("interlude_bring_materials_1"):
    +[Вот, погляди]
    Отлично! Думаю, эти материалы подойдут. Теперь я могу начать разработку устройства.
    ~invoke_dialogue_event("interlude_bring_materials_1")
    Кстати, Марк хотел тебя видеть. Говорил, у него для тебя какое-то важное послание.
        ++[Хорошо, пойду к нему. Пока!]
            Удачи! Если добудешь какие-нибудь новые реликвии, сразу тащи сюда. Я всегда рада тебя видеть!
                ->END
    }
    +[Пока нет, я работаю над этим.]
        Хорошо. Удачи!
            ->END

=== generic ===

Привет, есть какие-нибудь новости?
    +[Пока что нет. Я пойду.]
        Пока!
        ->END