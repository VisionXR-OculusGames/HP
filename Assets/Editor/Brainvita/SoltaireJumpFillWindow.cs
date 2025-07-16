using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SoltaireSlotJumpFillWindow : EditorWindow
{
    private GameObject slotsParent;
    private float idealDistance = 0.44f;
    private float tolerance = 0.01f;

    [MenuItem("Tools/Soltaire Jumpable Slot Helper")]
    public static void ShowWindow()
    {
        GetWindow<SoltaireSlotJumpFillWindow>("Jumpable Slot Filler");
    }

    private void OnGUI()
    {
        GUILayout.Label("Auto-fill Jumpable Slots for ALL", EditorStyles.boldLabel);

        slotsParent = (GameObject)EditorGUILayout.ObjectField("Slots Parent", slotsParent, typeof(GameObject), true);
        idealDistance = EditorGUILayout.FloatField("Ideal Jump Distance", idealDistance);
        tolerance = EditorGUILayout.FloatField("Tolerance (+/-)", tolerance);

        if (slotsParent == null)
        {
            EditorGUILayout.HelpBox("Assign the parent GameObject that holds all slots.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Auto-Fill Jumpable Slots for All"))
        {
            FillJumpableSlotsForAll();
        }
    }

    private void FillJumpableSlotsForAll()
    {
        SoltaireSlot[] allSlots = slotsParent.GetComponentsInChildren<SoltaireSlot>();

        foreach (SoltaireSlot currentSlot in allSlots)
        {
            List<SoltaireSlot> jumpList = new List<SoltaireSlot>();

            foreach (SoltaireSlot otherSlot in allSlots)
            {
                if (otherSlot == currentSlot) continue;

                float dist = Vector3.Distance(otherSlot.transform.localPosition, currentSlot.transform.localPosition);
                if (Mathf.Abs(dist - idealDistance) <= tolerance)
                {
                    jumpList.Add(otherSlot);
                }
            }

            Undo.RecordObject(currentSlot, "Assign Jumpable Slots");
            currentSlot.jumpableSlots = jumpList;
            EditorUtility.SetDirty(currentSlot);
        }

        Debug.Log($"✅ Filled jumpable slots for {allSlots.Length} slots using distance range {idealDistance - tolerance} to {idealDistance + tolerance}.");
    }
}
