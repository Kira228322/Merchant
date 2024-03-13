using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newTutorialPresentation", menuName = "Tutorial/Tutorial Presentation")]
public class TutorialPresentation : ScriptableObject
{
    [Serializable]
    public class TutorialSlide
    {
        public Sprite Image;
        [TextArea(3, 15)] public string Text;
    }

    public string Title;
    public string Summary;
    public List<TutorialSlide> Slides = new();
}
