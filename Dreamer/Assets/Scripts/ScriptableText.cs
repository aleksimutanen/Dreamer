using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct TimedText {
    public string text;
    public float timeToDisplay;
    public TimedText(string text, float timeToDisplay) {
        this.text = text;
        this.timeToDisplay = timeToDisplay;
    }
}

[CreateAssetMenu(fileName = "ScriptableText", menuName = "ScriptableText", order = 1)]
public class ScriptableText : ScriptableObject {
    public List<TimedText> texts;
}