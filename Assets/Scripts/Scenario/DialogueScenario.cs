using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StringEntry
{
    [TextArea]
    public string text;
}

[CreateAssetMenu(fileName = "DialogueScenario", menuName = "Scenario/DialogueScenario")]
public class DialogueScenario : Scenario
{
    public List<StringEntry> dialogues;
}
