INCLUDE ../../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "give_contract_to_northern_leader"):
    -> give_contract
-else:
    -> generic
}

=== give_contract ===
Здравствуйте, дорогой друг! 
+[Я с хорошими новостями. Гильдия Торговцев готова открыть торговый путь. Вот контракт.]
    Великолепно, просто потрясающе! Я знал, что могу на вас положиться.
    \*Одрин достаёт из мешка предмет странного вида и протягивает его вам*
    ~place_item("Реликвия Северного народа", 1, 0)
    ~invoke_dialogue_event("give_contract_to_northern_leader")
    Вот, прошу, как и договаривались. Это необходимая вам реликвия.
    Благодарю вас за помощь и желаю успехов в ваших начинаниях!
    ->END
    
=== generic ===
Здравствуйте, дорогой друг. Как я могу вам помочь?
    +[Уйти.]
        ->END