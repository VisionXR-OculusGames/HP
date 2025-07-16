using UnityEngine;
using UnityEditor;

public class MarbleCreatorWindow : EditorWindow
{
    private GameObject marbleParent;
    private GameObject marbleTemplate;
    private GameObject slotsParent;

    [MenuItem("Tools/Marble Creator")]
    public static void ShowWindow()
    {
        GetWindow<MarbleCreatorWindow>("Marble Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Marble Creator", EditorStyles.boldLabel);

        marbleParent = (GameObject)EditorGUILayout.ObjectField("Marble Parent", marbleParent, typeof(GameObject), true);
        marbleTemplate = (GameObject)EditorGUILayout.ObjectField("Marble Template", marbleTemplate, typeof(GameObject), false);
        slotsParent = (GameObject)EditorGUILayout.ObjectField("Slots Parent", slotsParent, typeof(GameObject), true);

        if (marbleParent == null || marbleTemplate == null || slotsParent == null)
        {
            EditorGUILayout.HelpBox("Assign Marble Parent, Marble Template, and Slots Parent.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Create Marbles at Slot Positions"))
        {
            CreateMarbles();
        }
    }

    private void CreateMarbles()
    {
        Undo.RegisterFullObjectHierarchyUndo(marbleParent, "Create Marbles");

        int count = slotsParent.transform.childCount;

        for (int i = 0; i < count; i++)
        {
            Transform slot = slotsParent.transform.GetChild(i);

            GameObject marble = Instantiate(marbleTemplate, slot.position, slot.rotation, marbleParent.transform);
            marble.name = $"Marble{i + 1}";
        }

        EditorUtility.SetDirty(marbleParent);
        Debug.Log($"? Created {count} marbles matching slot positions under '{marbleParent.name}'.");
    }
}
