using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerPrefsSettings))]
public class PlayerPrefsEditor : Editor
{
    private int addApplesAmount;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerPrefsSettings contr = (PlayerPrefsSettings) target;

        EditorGUILayout.LabelField("Current apples: " + contr.GetApples());
        EditorGUILayout.LabelField("Highscore: " + contr.GetHighscore());
        EditorGUILayout.LabelField("MaxLevel: " + contr.GetMaxLevel());

        if (GUILayout.Button("Reset Player Prefs"))
        {
            contr.ResetPlayerPrefs();
        }
        addApplesAmount = EditorGUILayout.IntField("Apples amount", addApplesAmount);
        if (GUILayout.Button("Add Apples"))
        {
            contr.AddApples(addApplesAmount);
        }
    }
}
