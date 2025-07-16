using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SoltaireSlotEditorWindow : EditorWindow
{
    private GameObject slotsParent;
    private float maxDistance = 0.22f;

    [MenuItem("Tools/Soltaire Slot Helper")]
    public static void ShowWindow()
    {
        GetWindow<SoltaireSlotEditorWindow>("Soltaire Slot Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Auto-fill Adjacent Slots for ALL", EditorStyles.boldLabel);

        slotsParent = (GameObject)EditorGUILayout.ObjectField("Slots Parent", slotsParent, typeof(GameObject), true);
        maxDistance = EditorGUILayout.FloatField("Max Distance", maxDistance);

        if (slotsParent == null)
        {
            EditorGUILayout.HelpBox("Assign the parent GameObject that holds all slots.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Auto-Fill Adjacent Slots for All"))
        {
            FillAdjacentSlotsForAll();
        }
    }

    private void FillAdjacentSlotsForAll()
    {
        SoltaireSlot[] allSlots = slotsParent.GetComponentsInChildren<SoltaireSlot>();

        foreach (SoltaireSlot currentSlot in allSlots)
        {
            List<SoltaireSlot> adjacentList = new List<SoltaireSlot>();

            foreach (SoltaireSlot otherSlot in allSlots)
            {
                if (otherSlot == currentSlot) continue;

                float dist = Vector3.Distance(otherSlot.transform.localPosition, currentSlot.transform.localPosition);
                if (dist <= maxDistance)
                {
                    adjacentList.Add(otherSlot);
                }
            }

            Undo.RecordObject(currentSlot, "Assign Adjacent Slots");
            currentSlot.adjacentSlots = adjacentList;
            EditorUtility.SetDirty(currentSlot);
        }

        Debug.Log($"? Filled adjacent slots for {allSlots.Length} slots.");
    }
}
