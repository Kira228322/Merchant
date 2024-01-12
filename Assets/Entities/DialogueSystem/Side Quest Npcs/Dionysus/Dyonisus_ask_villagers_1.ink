INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

Здрав будь, добрый молодец. Подсказать чего?
    {contains(activeQuests, "beginning_of_valor") && not is_goal_completed("beginning_of_valor", 0):
        +[Как у вас жизнь, нужна ли помощь с чем-нибудь?]
		    Да вроде справляемся кое-как. 
		    ~invoke_dialogue_event("beginning_of_valor_npc1")
		    Спасибо за предложение!
            ->END
    
    }
        +[Я пойду.]
            Удачи!