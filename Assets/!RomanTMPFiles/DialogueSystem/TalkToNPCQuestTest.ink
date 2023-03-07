INCLUDE MainInkLibrary.ink


-> greeting


=== greeting ===
Привет.
-> main

=== main ===
+ [У тебя есть для меня какая-то работа?]
    -> give_quest
+ [Мне пора идти.]
    -> END
+ {check_if_quest_completed("TestQuestTalkToPetrovichAboutBuhlo") && !check_if_quest_completed("TestQuestReturnAfterSpeakingWithPetrovich")}
    [Я поговорил с Петровичем, он придёт.]
    ->quest_completed
    
    
=== give_quest ===
{
 -!check_if_quest_active("TestQuestTalkToPetrovichAboutBuhlo") && !check_if_quest_completed("TestQuestTalkToPetrovichAboutBuhlo"):
    Да, я хотел предложить тебе одно дело. Не волнуйся, я заплачу.
    Нужно, чтобы ты сходил к моему другу-торговцу и передал ему следующее:
    "Приходи сегодня ко мне в 21:00, будем бухать. Есть самогон и водка"
    Ну, как думаешь, справишься?
    + [Легко.]
        #give_quest TestQuestTalkToPetrovichAboutBuhlo
        Ну вот и отлично.
        -> main
    + [Чёт как-то не хочется.]
        Ну, как хочешь.
        -> main

-else:
    Нет, сейчас ничего нету.
    -> main
}

=== quest_completed ===
    #invoke returned_after_talking_about_buhlo
Спасибо, что передал Петровичу мою просьбу. Вот твоя награда.
-> main