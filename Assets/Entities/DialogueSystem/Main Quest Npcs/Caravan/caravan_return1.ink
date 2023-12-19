INCLUDE ../../MainInkLibrary.ink

~temp activeQuests = get_activeQuestList()

{contains(activeQuests, "receive_nomad_reply"):
    ->get_reply
}
{contains(activeQuests, "send_resources_to_nomads"):
    ->send_resources
-else:
    ->generic
}

=== get_reply ===
А, чужак! Помню тебя. В этот раз Пустыня не была к нам благосклонна, но с помощью духов мы преодолели все преграды.
Совет Старейшин прочитал твоё письмо и рассмотрел твою просьбу. Мне неведомы все детали того, о чём беседовали Старейшины, но они передали тебе это письмо.
~invoke_dialogue_event("receive_nomad_reply")
~place_item("Письмо от кочевников", 1, 0)
->END

=== send_resources ===
Чем я могу помочь, чужак?
    +[У меня есть некоторые ресурсы, чтобы отправить твоим племенам.]
        ->select
    +[Я пойду.]
        Да пребудет с тобой духовная мудрость...
            ->END

=select
{not contains(activeQuests, "send_resources_to_nomads"):
Похоже, это всё? Хорошо. Караван скоро отправится. В этот раз я не прошу у тебя пожертвование.
Вижу, что ты и так помогаешь нашим поселениям. Спасибо тебе, чужак.
Жди нашего возвращения. Ты сдержал своё слово, и кочевники всегда держат своё.
->END
-else:
        Давай посмотрим, что там у тебя...
            {not is_goal_completed("send_resources_to_nomads", 0):
            ++[*Погрузить зелья*]
                ~open_item_container("send_resources_to_nomads", 0)
                ->select
            }
            {not is_goal_completed("send_resources_to_nomads", 1):
            ++[*Погрузить добычу с монстров*]
                ~open_item_container("send_resources_to_nomads", 1)
                ->select
            }
            {not is_goal_completed("send_resources_to_nomads", 2):
            ++[*Погрузить магические предметы*]
                ~open_item_container("send_resources_to_nomads", 2)
                ->select
            }
}

=== generic ===
Да направят тебя духи на верный путь, чужак.
->END