INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "ask_oldmage_about_new_relic"):
    -> bargain_about_relic
-else:
    -> generic
}

=== bargain_about_relic ===
+[Я слышал, что у тебя есть одна редкая реликвия. Ты можешь поделиться ей со мной?]
    Ах, молодой человек, не каждый день кто-то приходит в мою лавку с подобной просьбой. Магические реликвии - это не игрушки для каждого, знаешь ли?
        ++[Но ведь именно поэтому я и пришел! Я уверен, у тебя есть по-настоящему удивительные вещи.]
            ->bargain_about_relic.a
        ++[Я понимаю, что это ценные вещи, но мне действительно нужна помощь. Могу я рассчитывать на твою поддержку?]
            ->bargain_about_relic.a
= a
            Что ж, видимо ты не просто случайный посетитель. Но, знаешь ли, я всю жизнь коллекционирую магические реликвии - для меня это нечто особенное.
            Почему бы не поговорить об этом подробнее?
            +[Я всегда готов услышать истории о магии и таинственных предметах. Расскажи мне, пожалуйста!]
            ->bargain_about_relic.longExplanation
            +[Хорошо, я слушаю. Но пожалуйста, давай сразу к сути. У меня нет времени на долгие разговоры.]
            ->bargain_about_relic.shortExplanation
= longExplanation
            Каждая из этих реликвий обладает своей собственной историей и магической силой. Некоторые из них так древни, что их происхождение погружено в самые глубины времени.
             Эти реликвии - не просто предметы, они несут в себе древнюю магию, таинственную и могущественную. Люди часто забывают о том, как велика магия вокруг нас.
             Реликвии могут стать твоим верным спутником в путешествиях, раскрывая перед тобой тайны, о которых ты даже не мечтал.
             Но, как и с любой великой вещью, с ними нужно обращаться осторожно.
             ->shortExplanation
= shortExplanation
             Эх, нынче молодежь стала забывать о величии наших предков... Все стремятся обрести силу, но никто не задумывается, откуда она взялась.
             Ладно уж, у меня действительно есть реликвия, о который ты просишь меня, но она особенная. Я не могу отдать её просто так.
            +[Что же я должен сделать, чтобы получить её?]
                Я готов обменять эту реликвию на другую, но она находится в магическом лесу, далеко отсюда.
                ++[Магический лес? Звучит увлекательно. Я с радостью отправлюсь туда и принесу тебе реликвию.]
                    ->about_forest
                ++[Звучит опасно. Но, как я понимаю, других вариантов нет...]
                    ->about_forest
=about_forest
                    Вот и отлично! Реликвия, которая мне нужна - это огненный лист с особого магического дерева.
                    Ты найдешь магическое дерево в самом центре леса. Однако загвоздка заключается в том, что тот лес окружён неким магическим барьером, который никого не пропускает.
                    Я дам тебе магический свиток, который должен помочь тебе преодолеть этот барьер. Подойдя к магическому барьеру, просто прочитай заклинание со свитка, держа его в руках.
                    Над созданием этого свитка я трудился несколько лет и изучил огромное количество древних книг. В итоге мне удалось запечатать в этот свиток частичку магии волшебника, который создал тот барьер.
                    ...Уж если быть совсем честным с тобой, юноша, я не до конца уверен, сработает ли этот свиток. Что ж, даже если ничего не выйдет, это полезный опыт.
                    ~place_item("Свиток Альберика", 1, 0)
                    ~invoke_dialogue_event("ask_oldmage_about_new_relic")
                    Я надеюсь на тебя!
                    +[Не переживай, я сделаю всё, что в моих силах.]
                    ->finalize
                    +[Давай сюда свой свиток...]
                    ->finalize
=finalize
                    Ах да, совсем забыл. Магический лес находится в деревне Зарница, это довольно далеко отсюда. Не забудь взять с собой достаточно припасов.
                    Что ж, счастливого пути! Да будет твоя дорога безопасной и спокойной.
                    ->END
    


=== generic ===
Добро пожаловать, юный покупатель. Не желаешь взглянуть на мою коллекцию? У меня много интересных магических вещей.
+[Что у тебя есть интересного?]
    О, все мои предметы высочайшего качества! Взгляни, например, на этот магический Нагреватель. Он поможет тебе сохранить тепло даже в самых холодных местах, а также защищает от ледяной магии.
    Если тебе не нравится тепло, у меня есть для тебя Ледяная колба. Она способна охлаждать всё, что находится внутри неё.
    Для тех, кто жаждет, у меня есть Бездонный бокал. Однако помни, что силы природы не поддаются алчности!
    Вся коллекция доступна для тебя, мой юный друг. Выбирай, что тебе по душе.
        ++[Не знаю... Может быть, я подумаю и вернусь позже.]
            ->exit
+[Спасибо, я пойду]
    ->exit
    
=== exit ===
Обязательно возвращайся! Я всегда рад покупателям.
-> END