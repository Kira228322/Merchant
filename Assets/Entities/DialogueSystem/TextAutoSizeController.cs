using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAutoSizeController : MonoBehaviour
{
    public List<TMP_Text> TextObjects = new();

    public void Activate()
    {
        if (TextObjects == null || TextObjects.Count == 0)
            return;

        int candidateIndex = 0;
        float maxPreferredWidth = 0;

        for (int i = 0; i < TextObjects.Count; i++)
        {
            float preferredWidth = TextObjects[i].preferredWidth;
            if (preferredWidth > maxPreferredWidth)
            {
                maxPreferredWidth = preferredWidth;
                candidateIndex = i;
            }
        }

        // Force an update of the candidate text object so we can retrieve its optimum point size.
        TextObjects[candidateIndex].enableAutoSizing = true;
        TextObjects[candidateIndex].ForceMeshUpdate();
        float optimumPointSize = TextObjects[candidateIndex].fontSize;

        // Disable auto size on our test candidate
        TextObjects[candidateIndex].enableAutoSizing = false;

        // Iterate over all other text objects to set the point size
        for (int i = 0; i < TextObjects.Count; i++)
            TextObjects[i].fontSize = optimumPointSize;
    }

    private void Awake()
    {
        Activate();
    }
}
