using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SelectOption
{
    public StringEntry option;
    public Scenario scenario;
}

[CreateAssetMenu(fileName = "SelectScenario", menuName = "Scenario/SelectScenario")]
public class SelectScenario : Scenario
{
    public List<SelectOption> options;
}
