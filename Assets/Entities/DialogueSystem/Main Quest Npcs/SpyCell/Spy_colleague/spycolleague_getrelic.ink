INCLUDE ../../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "get_relic_from_agent"):
    ->get_relic
-else:
    ->generic
}

=== get_relic ===
Это снова ты!
    +[Я должен забрать у тебя некую посылку.]
        Отлично, ты как раз вовремя! Послушай внимательно, я скажу тебе кое-что важное!
        \*Ферзь наклоняется к вам и шепчет на ухо*
        У контрабандистов сейчас большой переполох. Кто-то сообщил о них страже, и сейчас они пытаются разойтись в разные стороны, чтобы запутать следы.
        Мне, как лояльному сотруднику, доверили вот эту коробку. Но я подслушал их разговоры. В этой коробке не обычная контрабанда! Это какая-то очень ценная магическая штуковина, ради которой они и вели свои дела здесь.
        Эту коробку мне приказали отдать тебе. Думаю, они хотят, чтобы ты вывез её за границу.
        В общем, на твоём месте я не стал бы вывозить её. Ты торговец, тебе она полезнее чем мне, наверняка ты легко найдешь, кому её продать.
            ++[Спасибо. Я найду способ отплатить тебе.]
                Не волнуйся об этом. Я всегда хотел помогать людям, как и ты.
                Обмануть обманщика в моём понимании - доброе дело.
                ~place_item("Захваченная реликвия", 1, 0)
                ~invoke_dialogue_event("get_relic_from_agent")
                Теперь прощай. Не знаю, увидимся ли мы ещё раз. Желаю удачи!
                    ->END


=== generic ===
Здравствуй, торговец. Как твои дела?
    +[Я пойду.]
        Успехов тебе!
        ->END