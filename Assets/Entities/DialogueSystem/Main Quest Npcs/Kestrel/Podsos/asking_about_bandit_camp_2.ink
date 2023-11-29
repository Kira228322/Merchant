INCLUDE ../../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList_universal()

Приветствую, парень!
{contains(activeQuests, "asking_about_bandit_camp"):
-> about_kestrel
-else:
-> generic
}

=== about_kestrel ===
+[Я ищу человека, который знает что-то о лагере бандитов неподалёку.]
    Зачем это тебе? Записаться к ним хочешь что-ли? Шучу я, глаза у тебя добрые.
    Я слышал, что в соседней деревне Стрежень живёт какой-то бывший бандит. Мутный тип, но можешь спросить у него.
    ++[Спасибо.]
        Удачи!
        ->END
=== generic ===
+[Я пойду.]
    Удачи!
    ->END