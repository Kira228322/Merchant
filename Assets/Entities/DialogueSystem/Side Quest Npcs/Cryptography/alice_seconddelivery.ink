INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains (activeQuests, "deliveries_return_with_box"):
    ->delivery_return
-else:
    ->generic
}

=== delivery_return ===
Снова здравствуй. Как там доставка? Как поживает Боб?
    
    +[У Боба всё хорошо. Он передал вам коробку.]
        Хм, подарок? Интересно...
        ~invoke_dialogue_event("deliveries_return_with_box")
        Вот твоё вознаграждение. Спасибо, что согласился помочь.
        ->END

=== generic ===
Я слушаю тебя.
    +[Я пойду.]
        До свидания.
        ->END
