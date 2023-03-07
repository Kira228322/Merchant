VAR quest_Taken = false
VAR quest_Completed = false

VAR affinity = 0

#check_if_quest_taken TestQuestReturnAfterSpeakingWithPetrovich

-> greeting


=== greeting ===
Привет.
~quest_Completed = quest_Taken
#check_if_quest_taken TestQuestTalkToPetrovichAboutBuhlo
//Вот эта строчка выше - костыль. Так делать нельзя, потому что
//эта команда check_if_quest_taken всегда перезаписывает одну переменную quest_Taken.
//То есть, quest_Taken станет true если взят любой из этих двух квестов.
//С помощью таких проверок я избежал создания второй переменной, но считаю это
//костылём. Тем не менее, я уже знаю как делать это
//через внешние C# функции, поэтому в будущем проблем не будет.
-> main
=== main ===
+ [У тебя есть для меня какая-то работа?]
    -> give_quest
+ [Мне пора идти.]
    -> END
* {quest_Completed}
    [Я поговорил с Петровичем, он придёт.]
    ->quest_completed
    
    
=== give_quest ===
{quest_Taken == false:
    Да, я хотел предложить тебе одно дело. Не волнуйся, я заплачу.
    Нужно, чтобы ты сходил к моему другу-торговцу и передал ему следующее:
    "Приходи сегодня ко мне в 21:00, будем бухать. Есть самогон и водка"
    Ну, как думаешь, справишься?
    + [Легко.]
        ~quest_Taken = true
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