INCLUDE MainInkLibrary.ink

->greeting

=== greeting ===

Привет, я ЖЭБА
-> general


=== general ===
~ temp questReady = is_questgiver_ready_to_give_quest()
~ temp randomQuestSummary = get_active_quest_for_this_npc()
~ temp questActive = randomQuestSummary != "null"

{
    - not questActive:
        {
            -questReady:
                +[Дай мне задание]
                    ~randomQuestSummary = get_random_questparams_of_questgiver()
                    ~add_quest(randomQuestSummary)
                    {
                        -randomQuestSummary == "zheba_strela":
                         #switch со всеми возможными квестами для этого нпс
                            Ок, принеси мне несколько стрел для того, чтобы я стреляла ими по принцам и влюбляла их в себя.
                         -randomQuestSummary == "null":
                              Чё за хуйня бля
                    }
                    -> general
        }
    - else:
        ~ temp requiredAmount = get_required_amount_for_quest(randomQuestSummary)
        ~ temp hasEnoughItems = check_if_has_items("Стрела", requiredAmount)
        {
            -hasEnoughItems:
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
    Ну пока.
    -> END