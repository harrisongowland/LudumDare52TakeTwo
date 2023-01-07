using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DiceSystem))]
public class DiceSystemEditor : Editor
{

    private DiceSystem system; 
    
    public override void OnInspectorGUI()
    {
        if (system == null)
        {
            system = target as DiceSystem;
        }
        
        base.OnInspectorGUI();

        if (GUILayout.Button("Test roll"))
        {
            system.Roll();
        }
    }
}
