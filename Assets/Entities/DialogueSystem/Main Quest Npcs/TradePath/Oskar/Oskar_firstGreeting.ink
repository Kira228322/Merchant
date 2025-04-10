INCLUDE ../../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "suggest_opening_inn"):
    ->suggest_opening
-else:
    ->generic
}

=== suggest_opening ===
Привет, добрый странник... Как твои дела в эти мрачные дни?
+[Приветствую. Ты выглядишь грустным, что случилось?]
    Да ничего, просто тоска напала. Вспоминаю свою таверну, как она была всегда живой и наполненной радостью и уютом. Теперь это просто пустое здание с застывшими воспоминаниями...
            ++[У меня есть интересная информация для тебя. В этом регионе скоро откроется торговый путь.]
                Торговый путь? Но как это поможет?
                    +++[Да ведь сюда хлынут толпы торговцев и путешественников. Люди будут ускать уюта после долгих странствий.]
                        Я надеюсь, ты не играешь со мной шутки, добрый странник... Что ж, может быть действительно стоит дать таверне второй шанс.
                        Если я продам часть своих вещей и достану свои накопления...
                        ~invoke_dialogue_event("suggest_opening_inn")
                        Спасибо тебе за вдохновение. Если дело выгорит, приходи в мою таверну позже. Я угощу тебя кружечкой чего-то покрепче!
                        ->END

=== generic ===
Здравствуй, путник. Боюсь, я ничем не могу тебе помочь..
    +[Расскажи о себе.]
        Что рассказывать? Меня зовут Оскар. Раньше я держал таверну в этих краях.
        Дела шли хорошо, у меня было всё: семья, отличная работа, радостная жизнь.
        Потом всё покатилось под откос: люди перестали приходить в таверну. Чтобы расплатиться с долгами, мне пришлось продать свою повозку и лошадей. 
        После этого от меня ушла жена, забрав детей... Она не хотела жить в бедности, и была права...
        ...Впрочем, прости, что я вывалил на тебя свои проблемы. Не знаю, что на меня нашло...
            ++[Да ладно, не расстраивайся. Нужно смотреть на жизнь с позитивом!]
                Эх, что ты понимаешь, путник...
                    ->END
            ++[Завтра будет лучше, чем вчера. Поверь мне.]
                Хотел бы я поверить тебе... Спасибо за тёплые слова.
                    ->END
                
    +[Мне пора идти.]
        Что ж, прощай. У тебя есть дела поважнее, чем болтать с таким плаксой, как я...
        ->END