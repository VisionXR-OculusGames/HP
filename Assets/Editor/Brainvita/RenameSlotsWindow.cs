using UnityEngine;
using UnityEditor;

public class RenameSlotsWindow : EditorWindow
{
    private GameObject targetParent;

    [MenuItem("Tools/Rename Children to Slots")]
    public static void ShowWindow()
    {
        GetWindow<RenameSlotsWindow>("Rename Slots");
    }

    private void OnGUI()
    {
        GUILayout.Label("Rename Children", EditorStyles.boldLabel);

        targetParent = (GameObject)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(GameObject), true);

        if (targetParent == null)
        {
            EditorGUILayout.HelpBox("Assign a GameObject that has children you want to rename.", MessageType.Info);
            return;
        }

        if (GUILayout.Button("Rename All Children to Slot0, Slot1..."))
        {
            RenameChildren();
        }
    }

    private void RenameChildren()
    {
        Transform parentTransform = targetParent.transform;
        int childCount = parentTransform.childCount;

        Undo.RegisterFullObjectHierarchyUndo(targetParent, "Rename Children"); // allows undo

        for (int i = 0; i < childCount; i++)
        {
            Transform child = parentTransform.GetChild(i);
            child.name = $"Slot{i+1}";
        }

        Debug.Log($"Renamed {childCount} children of '{targetParent.name}' to Slot0...Slot{childCount - 1}");
        EditorUtility.SetDirty(targetParent);
    }
}
