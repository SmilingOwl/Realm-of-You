using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class VariableStorage : VariableStorageBehaviour
{
    private Dictionary<string, Yarn.Value> variables = new Dictionary<string, Yarn.Value> ();

    public override void SetValue(string variableName, Yarn.Value value) {
        variables[variableName] = new Yarn.Value(value);
    }

    public override Yarn.Value GetValue(string variableName) {
        if (variables.ContainsKey(variableName) == false)
            return Yarn.Value.NULL;
        return variables [variableName];
    }

    public override void ResetToDefaults () {
        foreach(KeyValuePair<string, bool> decision in GameController.instance.decisions) {
            Yarn.Value value = new Yarn.Value(decision.Value);
            this.SetValue("$"+decision.Key, value);
        }
    }
}
