INCLUDE MainInkLibrary.ink

->greeting

=== greeting ===

Привет, я ЖЭБА
-> general


=== general ===
~ temp questState = get_quest_state("zheba_strela")

{
    -questState == "null":
    #TODO: -questState not "null" : check externally whether QuestGiver is ready to give quest
        +[Дай мне задание]
            ~add_quest("zheba_strela")
            Ок, принеси мне несколько стрел для того, чтобы я стреляла ими по принцам и влюбляла их в себя.
            -> general
    -questState == "Active":
        ~ temp requiredAmount = get_required_amount_for_quest("zheba_strela")
        ~ temp hasEnoughItems = check_if_has_items("Стрела", requiredAmount)
        {hasEnoughItems:
        +[Я принес тебе твои стрелы, Жэба]
            ~invoke_dialogue_event("zheba_give_arrows")
            Спасибо!
            -> general
        -else:
        +[Я все ещё в процессе поиска твоих стрел, Жэба]
            Ничего страшного, я никуда не денусь
            -> general
        }
}
    +[Пока]
        -> END