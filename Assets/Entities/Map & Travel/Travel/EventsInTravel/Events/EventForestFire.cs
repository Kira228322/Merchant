using System.Collections.Generic;
using UnityEngine;

public class EventForestFire : EventInTravel
{
    [SerializeField] private List<GameObject> _destroyedObjects;
    public override void SetButtons()
    {
        ButtonsLabel.Add("Помочь в тушении");
        ButtonsLabel.Add("Он и сам справится");
        SetInfoButton("На тушение пожара уйдет какое-то время и вы потратите свои силы.");
    }

    public override void OnButtonClick(int n)
    {
        switch (n)
        {
            case 0:
                GameTime.TimeSkip(0, 1, Random.Range(10, 30));
                Player.Instance.Needs.CurrentHunger -= 10;
                Player.Instance.Needs.CurrentSleep -= 5;
                int exp = Random.Range(3, 5);
                Player.Instance.Experience.AddExperience(exp);
                _eventWindow.ChangeDescription($"Пожар был успешно потушен! Вы получили {exp} опыта!");

                foreach (var obj in _destroyedObjects)
                    Destroy(obj);

                break;
            case 1:
                _eventWindow.ChangeDescription("Вы сделали вид, что ничего не заметили и поехали дальше.");
                break;
        }
    }
}
