INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

Здрав будь, добрый молодец.
{contains(activeQuests, "asking_about_bandit_camp"):
-> about_kestrel
-else:
-> generic
}

=== about_kestrel ===
+[Я ищу человека, который знает что-то о лагере бандитов неподалёку.]
    Охо, да это ты про нашего Сокола, не иначе? Он бывший бандит, живёт на краю деревни. Весь честной народ его побаивается, мало ли что у него в голове. Наверняка он тебе поможет.
    ++[Спасибо за помощь, я пойду.]
        Счастливо!
        ->END
=== generic ===
+[Я пойду.]
    Счастливо!
    ->END