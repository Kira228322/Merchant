INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "bridge_2nd_batch"):
    ->awaiting_resources
-else:
    ->greeting
}

=== awaiting_resources ===
Привет! Какие новости?
{has_enough_items("bridge_2nd_batch"):
    +[Я принёс тебе вторую партию досок.]
        Отлично, значит вскоре мост будет уже готов!
        ~invoke_dialogue_event("bridge_2nd_batch")
        Возвращайся через пару дней, чтобы увидеть результат наших с тобой стараний. Ты не пожалеешь!
        ->END
-else:
    +[Я всё ещё ищу доски для тебя.]
        Ничего страшного, понимаю, в округе не так много досок. Я буду ждать!
        ->END
}

=== greeting ===
\*Стив закончил постройку каркаса, и сейчас трудится над завершением моста. Очевидно, мост уже почти готов, и вскоре через него можно будет перейти* 
->END
}
