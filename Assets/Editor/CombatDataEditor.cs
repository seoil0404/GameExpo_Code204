using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CombatData))]
public class CombatDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CombatData data = (CombatData)target;

        if(GUILayout.Button("Add All Treasure Data"))
        {
            data.AddAllTreasureData();
        }
    }
}
