INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

Привет. Чего-то хотел?
    {contains(activeQuests, "beginning_of_valor") && not is_goal_completed("beginning_of_valor", 2):
        +[Как у вас жизнь, нужна ли помощь с чем-нибудь?]
		    Ох, беда пришла в мою семью... Дядя мой заболел, да лёг, совсем ходить не может. Надо идти в соседнюю деревню за знахарем або травником, а боязно.
		    Говорят, та дорога стала очень опасной...
		        ++[Я знаю человека, который проведёт тебя.]
		            Правда? Ох, выручите вы меня и мою семью!
                        ~invoke_dialogue_event("beginning_of_valor_npc3")
                        //Если игрок нашёл этого типа раньше других, ему не нужно говорить с другими (пустышками)
                        ~invoke_dialogue_event_universal("beginning_of_valor_npc1")
                        ~invoke_dialogue_event_universal("beginning_of_valor_npc2")
                            Спасибо, я буду ждать твоего приятеля здесь. 
                                ->END
    
    }
        +[Я пойду.]
            Удачи!