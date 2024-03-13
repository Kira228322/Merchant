INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

Чего тебе?
    {contains(activeQuests, "beginning_of_valor") && not is_goal_completed("beginning_of_valor", 1):
        +[Как у вас жизнь, нужна ли помощь с чем-нибудь?]
		    Ишь ты какой хитрый! Я тебе скажу, а ты денег за помощь потребуешь!
		    ~invoke_dialogue_event("beginning_of_valor_npc2")
		    Иди гуляй, ходит тут, разнюхивает!
            ->END
    
    }
        +[Я пойду.]
            Иди-иди, нечего тебе тут делать...