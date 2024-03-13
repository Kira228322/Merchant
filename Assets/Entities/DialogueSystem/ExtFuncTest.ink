INCLUDE MainInkLibrary.ink

->greeting

=== greeting ===
Диалог пошел
~ temp questState = get_quest_state("external_test")
~ temp isTaken = check_if_quest_has_been_taken("external_test")
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
-> greeting


+[Покинуть диалог]
    Конец диалога
    ->END