using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct TimedText {
    public string text;
    public float timeToDisplay;
}

[CreateAssetMenu(fileName = "ScriptableText", menuName = "ScriptableText", order = 1)]
public class ScriptableText : ScriptableObject {
    public List<TimedText> texts;
}