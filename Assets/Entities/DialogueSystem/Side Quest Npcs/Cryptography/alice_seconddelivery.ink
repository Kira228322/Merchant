INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains (activeQuests, "deliveries_return_with_box"):
    ->delivery_return
-else:
    ->generic
}

=== delivery_return ===
Снова здравствуйте. Как там доставка? Как поживает Боб?
    
    +[У Боба всё хорошо. Он передал вам коробку.]
        Хм, подарок? Интересно...
        ~invoke_dialogue_event("deliveries_return_with_box")
        Вот ваше вознаграждение. Спасибо, что согласились помочь.
        ->END

=== generic ===
Я вас слушаю.
    +[Я пойду.]
        До свидания.
        ->END
