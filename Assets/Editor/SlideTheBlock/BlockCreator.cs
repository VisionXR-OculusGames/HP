#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class BlockCreator : EditorWindow
{
    private GameObject mainBlockPrefab;
    private GameObject sideBlockPrefab;
    private GameObject blockParent;

    private int blockLength = 2; // 2 or 3
    private BlockType blockType = BlockType.Main;
    private float depth = 0.02f;
    private float scaleFactor = 0.2f;

    [SerializeField]
    private List<GameObject> selectedSlotObjects = new List<GameObject>();
    private List<BlockSlot> selectedSlots;

    private enum BlockType { Main, Side }

    [MenuItem("Tools/Block Creator")]
    public static void ShowWindow()
    {
        GetWindow<BlockCreator>("Block Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Block Generator", EditorStyles.boldLabel);

        mainBlockPrefab = (GameObject)EditorGUILayout.ObjectField("Main Block Prefab", mainBlockPrefab, typeof(GameObject), false);
        sideBlockPrefab = (GameObject)EditorGUILayout.ObjectField("Side Block Prefab", sideBlockPrefab, typeof(GameObject), false);
        blockParent = (GameObject)EditorGUILayout.ObjectField("Block Parent", blockParent, typeof(GameObject), true);

        GUILayout.Space(10);
        blockType = (BlockType)EditorGUILayout.EnumPopup("Block Type", blockType);
        blockLength = EditorGUILayout.IntPopup("Block Length", blockLength, new[] { "2 Slots", "3 Slots" }, new[] { 2, 3 });
        depth = EditorGUILayout.FloatField("Block Depth", depth);
        scaleFactor = EditorGUILayout.FloatField("Scale Reduction Factor", scaleFactor);

        GUILayout.Space(10);
        SerializedObject so = new SerializedObject(this);
        SerializedProperty slotListProp = so.FindProperty("selectedSlotObjects");
        EditorGUILayout.PropertyField(slotListProp, new GUIContent($"Select {blockLength} Slots"), true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Create Block"))
        {
            selectedSlots = new List<BlockSlot>();
            foreach (var go in selectedSlotObjects)
            {
                if (go != null)
                {
                    BlockSlot bs = go.GetComponent<BlockSlot>();
                    if (bs != null) selectedSlots.Add(bs);
                }
            }

            if ((blockType == BlockType.Main && !mainBlockPrefab) ||
                (blockType == BlockType.Side && !sideBlockPrefab) ||
                selectedSlots.Count != blockLength ||
                blockParent == null)
            {
                Debug.LogError("? Please fill all fields and select correct number of slots.");
                return;
            }

            CreateBlock();
        }
    }

    void CreateBlock()
    {
        // Sort slots by position for consistency
        selectedSlots.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

        BlockSlot leftSlot = selectedSlots[0];
        BlockSlot rightSlot = selectedSlots[selectedSlots.Count-1];
       

        Vector3 center = (selectedSlots[0].transform.position + selectedSlots[selectedSlots.Count-1].transform.position) / 2f;
        float spacing = Vector3.Distance(selectedSlots[0].transform.position, selectedSlots[1].transform.position);

        bool isHorizontal = Mathf.Abs(leftSlot.transform.position.x - rightSlot.transform.position.x) >
                            Mathf.Abs(leftSlot.transform.position.z - rightSlot.transform.position.z);

        float unit = spacing ;
        spacing *= selectedSlots.Count;
        Vector3 size = isHorizontal
            ? new Vector3(spacing - unit * scaleFactor, unit - unit * scaleFactor, depth)
            : new Vector3(unit - unit * scaleFactor, spacing - unit * scaleFactor, depth);

        // Create block
        GameObject prefab = blockType == BlockType.Main ? mainBlockPrefab : sideBlockPrefab;
        GameObject blockGO = (GameObject)PrefabUtility.InstantiatePrefab(prefab, blockParent.transform);
        blockGO.transform.position = center;
        blockGO.transform.localScale = size;

        Block block = blockGO.GetComponent<Block>();
        block.left = selectedSlots[0];
        block.right = selectedSlots[selectedSlots.Count-1];
        block.blockOrientation = isHorizontal ? Orientation.Horizontal : Orientation.Vertical;

      
        if (selectedSlots.Count == 3)
        {
            block.middle = selectedSlots[1];
        }

        // Mark all slots as occupied
        foreach (var slot in selectedSlots)
        {
            slot.isSlotOccupied = true;
        }

        Debug.Log($"? {blockType} block created across {blockLength} slots.");
    }
}
#endif
