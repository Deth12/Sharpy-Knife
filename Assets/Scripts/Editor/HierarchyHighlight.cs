using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HierarchyHighlight : MonoBehaviour
{
    static HierarchyHighlight()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        
        if (obj != null && obj.name.StartsWith("--", System.StringComparison.Ordinal))
        {
            EditorGUI.DrawRect(selectionRect, Color.grey);
            EditorGUI.DropShadowLabel(selectionRect, obj.name.Replace("-", "").ToString());
        }
    }
}
