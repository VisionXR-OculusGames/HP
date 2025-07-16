#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ClearChildrenEditor : EditorWindow
{
    [SerializeField]
    private List<GameObject> parentObjects = new List<GameObject>();

    [MenuItem("Tools/Clear Children")]
    public static void ShowWindow()
    {
        GetWindow<ClearChildrenEditor>("Clear Children");
    }

    private void OnGUI()
    {
        GUILayout.Label("Remove All Children of Selected Parents", EditorStyles.boldLabel);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty parentListProp = so.FindProperty("parentObjects");
        EditorGUILayout.PropertyField(parentListProp, new GUIContent("Parent Objects"), true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Clear Children"))
        {
            ClearAllChildren();
        }
    }

    private void ClearAllChildren()
    {
        foreach (GameObject parent in parentObjects)
        {
            if (parent == null) continue;

            List<GameObject> toDestroy = new List<GameObject>();

            foreach (Transform child in parent.transform)
            {
                toDestroy.Add(child.gameObject);
            }

            foreach (GameObject child in toDestroy)
            {
                Undo.DestroyObjectImmediate(child);
            }

            Debug.Log($"? Cleared {toDestroy.Count} children from {parent.name}");
        }
    }
}
#endif
