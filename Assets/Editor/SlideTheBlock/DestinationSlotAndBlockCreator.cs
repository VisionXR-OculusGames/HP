#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DestinationSlotAndBlockCreator : EditorWindow
{
    private GameObject destinationBlockPrefab;
    private GameObject slotParent;
    private GameObject blockParent;
    private GameObject referenceSlot;

    private GameObject slotA; // for spacing calculation
    private GameObject slotB;

    private int refRow = 1;
    private int refCol = 4;

    [MenuItem("Tools/Destination Slot & Block Creator")]
    public static void ShowWindow()
    {
        GetWindow<DestinationSlotAndBlockCreator>("Destination Block Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Destination Block and Adjacent Slots", EditorStyles.boldLabel);

        destinationBlockPrefab = (GameObject)EditorGUILayout.ObjectField("Destination Block Prefab", destinationBlockPrefab, typeof(GameObject), false);
        slotParent = (GameObject)EditorGUILayout.ObjectField("Slot Parent", slotParent, typeof(GameObject), true);
        blockParent = (GameObject)EditorGUILayout.ObjectField("Block Parent", blockParent, typeof(GameObject), true);
        referenceSlot = (GameObject)EditorGUILayout.ObjectField("Reference Slot", referenceSlot, typeof(GameObject), true);

        GUILayout.Space(10);
        GUILayout.Label("Auto-Calculate Spacing From:");
        slotA = (GameObject)EditorGUILayout.ObjectField("Slot A (left)", slotA, typeof(GameObject), true);
        slotB = (GameObject)EditorGUILayout.ObjectField("Slot B (right)", slotB, typeof(GameObject), true);

        refRow = EditorGUILayout.IntField("Reference Row", refRow);
        refCol = EditorGUILayout.IntField("Reference Column", refCol);

        if (GUILayout.Button("Create Destination Setup"))
        {
            if (!destinationBlockPrefab || !slotParent || !blockParent || !referenceSlot || !slotA || !slotB)
            {
                Debug.LogError("? Please assign all required references.");
                return;
            }

            CreateSlotsAndBlock();
        }
    }

    void CreateSlotsAndBlock()
    {
        float unitSpacing = Vector3.Distance(slotA.transform.position, slotB.transform.position);
        Vector3 basePos = referenceSlot.transform.position;
        Vector3 offset = Vector3.right * unitSpacing;

        Vector3 pos1 = basePos + offset;             // Dest1 at 1,5
        Vector3 pos2 = basePos + offset * 2f;        // Dest2 at 1,6
        Vector3 center = (pos1 + pos2) / 2f;

        // Slot 1 (1,5)
        GameObject slot1 = new GameObject("Dest1");
        slot1.transform.position = pos1;
        slot1.transform.SetParent(slotParent.transform);
        var s1 = slot1.AddComponent<BlockSlot>();
        s1.row = refRow;
        s1.column = refCol + 1;

        // Slot 2 (1,6)
        GameObject slot2 = new GameObject("Dest2");
        slot2.transform.position = pos2;
        slot2.transform.SetParent(slotParent.transform);
        var s2 = slot2.AddComponent<BlockSlot>();
        s2.row = refRow;
        s2.column = refCol + 2;

        // Destination block
        GameObject block = (GameObject)PrefabUtility.InstantiatePrefab(destinationBlockPrefab, blockParent.transform);
        block.transform.position = center;

        // After placing the block at center...
        float spacing = Vector3.Distance(pos1, pos2);
        float unit = Vector3.Distance(slotA.transform.position, slotB.transform.position); // same as spacing between original slots

        float scaleFactor = 0.2f;
        float depth = 0.02f;

        Vector3 size = new Vector3(
            2 * spacing - spacing * scaleFactor,  // width to stretch across slots
            unit - unit * scaleFactor,           // height same as slot with reduction
            depth                                // thickness
        );

        block.transform.localScale = size;
        Debug.Log($"? Created Dest1 (1,{refCol + 1}) and Dest2 (1,{refCol + 2}) with block centered.");

        // connecting destination slots and others

        s2.left = s1;
        s1.right = s2;

        s1.left = referenceSlot.GetComponent<BlockSlot>();
        referenceSlot.GetComponent<BlockSlot>().right = s1;
    }
}
#endif
