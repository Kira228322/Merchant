INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "bring_notes_to_laura"):
    -> bring_notes
-else:
    -> generic
}

=== bring_notes ===
Ну как, удалось ли найти что-то в убежище отца?
    {has_enough_items("bring_notes_to_laura"):
    +[*Показать Лауре найденные заметки*]
        Хм, хм... Похоже, кое-что из этого может иметь отношение к нашей проблеме... Мне нужно время, чтобы разобраться во всём этом.
        ~invoke_dialogue_event("bring_notes_to_laura")
        Если найдешь какие-либо новые реликвии, приноси их сюда. Я всегда рада тебя видеть!
            -> END
    }
=== generic ===
Привет. Есть какие-либо новости?
    +[Пока ничего.]
        Что ж, спасибо что зашёл!
            ->END