using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SlotAdjacentAndJumpableFill : EditorWindow
{
    private GameObject slotsParent;
    private float jumpableDistance = 0.44f;
    private float adjacentDistance = 0.22f;
    private float tolerance = 0.01f;

    [MenuItem("Tools/Slot Adjacent and Jumpable Fill")]
    public static void ShowWindow()
    {
        GetWindow<SlotAdjacentAndJumpableFill>("Slot Auto-Fill");
    }

    private void OnGUI()
    {
        GUILayout.Label("Auto-Fill Jumpable & Adjacent Slots", EditorStyles.boldLabel);

        slotsParent = (GameObject)EditorGUILayout.ObjectField("Slots Parent", slotsParent, typeof(GameObject), true);
        jumpableDistance = EditorGUILayout.FloatField("Jumpable Distance", jumpableDistance);
        adjacentDistance = EditorGUILayout.FloatField("Adjacent Distance", adjacentDistance);
        tolerance = EditorGUILayout.FloatField("Tolerance", tolerance);

        if (slotsParent == null)
        {
            EditorGUILayout.HelpBox("Assign the parent GameObject that holds all slots.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Auto-Fill Slots"))
        {
            FillAllSlotConnections();
        }
    }

    private void FillAllSlotConnections()
    {
        SoltaireSlot[] allSlots = slotsParent.GetComponentsInChildren<SoltaireSlot>();

        foreach (SoltaireSlot currentSlot in allSlots)
        {
            List<SoltaireSlot> jumpables = new List<SoltaireSlot>();
            List<SoltaireSlot> adjacents = new List<SoltaireSlot>();

            Vector3 currentPos = currentSlot.transform.localPosition;

            // STEP 1: Find all valid jumpable slots
            foreach (SoltaireSlot other in allSlots)
            {
                if (other == currentSlot) continue;

                float dist = Vector3.Distance(currentPos, other.transform.localPosition);
                if (Mathf.Abs(dist - jumpableDistance) <= tolerance)
                {
                    jumpables.Add(other);
                }
            }

            // STEP 2: For each jumpable, find nearest midpoint slot
            foreach (SoltaireSlot jumpable in jumpables)
            {
                Vector3 midPoint = (currentPos + jumpable.transform.localPosition) * 0.5f;
                SoltaireSlot nearestMid = FindNearestSlot(midPoint, allSlots, currentSlot, jumpable);

                if (nearestMid != null)
                {
                    adjacents.Add(nearestMid);
                }
                else
                {
                    adjacents.Add(null); // To keep index aligned
                }
            }

            Undo.RecordObject(currentSlot, "Assign Jumpable and Adjacent Slots");
            currentSlot.jumpableSlots = jumpables;
            currentSlot.adjacentSlots = adjacents;
            EditorUtility.SetDirty(currentSlot);
        }

        Debug.Log("? Filled jumpable and adjacent slots for all.");
    }

    private SoltaireSlot FindNearestSlot(Vector3 position, SoltaireSlot[] allSlots, SoltaireSlot exclude1, SoltaireSlot exclude2)
    {
        float minDist = float.MaxValue;
        SoltaireSlot nearest = null;

        foreach (SoltaireSlot s in allSlots)
        {
            if (s == exclude1 || s == exclude2) continue;

            float dist = Vector3.Distance(position, s.transform.localPosition);
            if (dist <= tolerance && dist < minDist)
            {
                minDist = dist;
                nearest = s;
            }
        }

        return nearest;
    }
}
