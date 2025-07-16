using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RenameChildrenHelper))]
public class RenameChildrenHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RenameChildrenHelper script = (RenameChildrenHelper)target;

        if (GUILayout.Button("Rename Children to Slot + ID"))
        {
            if (script.targetParent == null)
            {
                Debug.LogWarning("Target Parent is not assigned.");
                return;
            }

            Transform parent = script.targetParent.transform;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                child.name = $"Slot{i+1}";
            }

            Debug.Log($"Renamed {parent.childCount} children of '{parent.name}'.");
        }
    }
}
