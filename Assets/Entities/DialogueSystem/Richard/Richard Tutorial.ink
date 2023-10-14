INCLUDE ../MainInkLibrary.ink
->greeting


=== greeting ===

Кого там нелегкая принесла?

~temp questSummaries = get_activeQuestList()
{contains(questSummaries, "tutorial_talk_to_richard"):
    +[Привет, ты Ричард? Я ищу своего отца. Он сказал, что пошёл забрать какую-то вещь у бандитов]
        -> about_relic
}
{contains(questSummaries, "tutorial_buy_relic_from_richard"):
    +[Я готов купить у тебя вещь моего отца. (200 золотых)]
        -> buy_relic
}
+[Мне пора идти.]
    Пока-пока.
    -> END

=== about_relic ===

Отца? Ну и ну, не знал что у того пройдохи были дети.
Да, я знаю про кого ты говоришь. Не хочу тебя расстраивать, пацан, но твоего отца схватили бандиты.
+[Как это произошло?]
    Да очень просто - в тот момент я ехал с ним. Бандиты устроили на нас засаду. Я смог сбежать, а твой батя - нет.
    Собственно говоря, он отдал мне ту штуку, что вёз, чтобы она не досталась бандитам. Не знаю, для чего она нужна, но выглядит красиво.
        ++[Отдашь её мне?]
            Ха-ха, ещё чёго! Просто так я тебе ничего не отдам.
            Пацан, ты знаешь поговорку "деньги правят миром"? За определённую плату мы могли бы договориться.
            Да не трясись, я даже сделаю тебе скидку, потому что твой батя меня спас.
            ~invoke_dialogue_event("tutorial_talk_to_richard")
            ~add_quest("tutorial_buy_relic_from_richard")
                ->END
    
=== buy_relic ===
Тогда давай сюда деньги.
{has_money(200) and check_if_can_place_item("tutorial_relic", 1):
    +[(Отдать 200 золота)]
        ~change_player_money(-200)
        ~invoke_dialogue_event("tutorial_buy_relic_from_richard")
        ~place_item("tutorial_relic", 1, 0)
        Вот это другой разговор. На, держи, мне она без толку.
        ->END
}
    +[Я передумал.]
        ->END

