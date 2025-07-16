using UnityEngine;
using UnityEditor;

public class MarbleAddWindow : EditorWindow
{
    private GameObject slotsParent;
    private GameObject marblesParent;

    [MenuItem("Tools/Marble Slot Assigner")]
    public static void ShowWindow()
    {
        GetWindow<MarbleAddWindow>("Marble Assigner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Assign Marbles to Slots", EditorStyles.boldLabel);

        slotsParent = (GameObject)EditorGUILayout.ObjectField("Slots Parent", slotsParent, typeof(GameObject), true);
        marblesParent = (GameObject)EditorGUILayout.ObjectField("Marbles Parent", marblesParent, typeof(GameObject), true);

        if (slotsParent == null || marblesParent == null)
        {
            EditorGUILayout.HelpBox("Assign both Slots and Marbles parents.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Assign Marbles by Order"))
        {
            AssignMarbles();
        }
    }

    private void AssignMarbles()
    {
        Transform slotRoot = slotsParent.transform;
        Transform marbleRoot = marblesParent.transform;

        int slotCount = slotRoot.childCount;
        int marbleCount = marbleRoot.childCount;

        if (slotCount != marbleCount)
        {
            Debug.LogWarning($"⚠ Slot count ({slotCount}) and Marble count ({marbleCount}) do not match!");
            return;
        }

        for (int i = 0; i < slotCount; i++)
        {
            Transform slot = slotRoot.GetChild(i);
            Transform marble = marbleRoot.GetChild(i);

            SoltaireSlot slotScript = slot.GetComponent<SoltaireSlot>();
            if (slotScript != null)
            {
                Undo.RecordObject(slotScript, "Assign Marble Reference");
                slotScript.marble = marble.gameObject;
                EditorUtility.SetDirty(slotScript);
            }
        }

        Debug.Log($"✅ Assigned {slotCount} marbles to slots based on order.");
    }
}
