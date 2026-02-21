using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenarioList", menuName = "Scenario/ScenarioList")]
public class ScenarioList : ScriptableObject
{
    public List<Scenario> scenarios;
}
