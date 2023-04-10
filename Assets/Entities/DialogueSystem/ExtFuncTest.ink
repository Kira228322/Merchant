INCLUDE MainInkLibrary.ink

->greeting

=== greeting ===
Диалог пошел
~ temp currentAffinity = get_affinity_here()
~ temp questState = get_quest_state("external_test")
~ temp isTaken = check_if_quest_has_been_taken("external_test")
~ temp currentAffinityByName = get_affinity_by_name("Dialogue Test")
~set_color("yellow")
{set_color("red")}   //void функции можно вызывать и с '~function' и с '{function}'

~debug_log(questState)

{
    -questState == "null":
        +[Дай карту]
            ~add_quest("external_test")
            Даю карту
            -> greeting
    -questState == "Active":
        +[Выполнить Квест]
            ~invoke_dialogue_event("external_talk_success")
            Выполнился
            -> greeting
        +[Зафейлить Квест]
            ~invoke_dialogue_event("external_talk_fail")
            Зафейлился
            -> greeting
    -questState == "Completed" || questState == "RewardUncollected":
        Похоже ты уже выполнил квест
    -questState == "Failed":
        Похоже ты зафейлил квест, гандон
}
+[Affinity + 10]

~ add_affinity_here(5)
~ add_affinity("Dialogue Test", 5)
-> greeting


+[Покинуть диалог]
    Конец диалога
    ->END