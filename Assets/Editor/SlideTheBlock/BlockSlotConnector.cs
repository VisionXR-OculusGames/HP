#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class BlockSlotConnector : EditorWindow
{
    private GameObject slotParent;
    private int gridSize = 4;

    [MenuItem("Tools/Block Slot Connector")]
    public static void ShowWindow()
    {
        GetWindow<BlockSlotConnector>("Block Slot Connector");
    }

    void OnGUI()
    {
        GUILayout.Label("Assign Neighbors to Block Slots", EditorStyles.boldLabel);

        slotParent = (GameObject)EditorGUILayout.ObjectField("Slot Parent", slotParent, typeof(GameObject), true);
        gridSize = EditorGUILayout.IntSlider("Grid Size", gridSize, 2, 10);

        if (GUILayout.Button("Assign Connections"))
        {
            if (slotParent == null)
            {
                Debug.LogError("Please assign the parent GameObject.");
                return;
            }

            AssignConnections();
        }
    }

    void AssignConnections()
    {
        // Build 2D array for fast access
        BlockSlot[,] grid = new BlockSlot[gridSize + 1, gridSize + 1]; // +1 to use 1-based indexing

        BlockSlot[] slots = slotParent.GetComponentsInChildren<BlockSlot>();

        foreach (BlockSlot slot in slots)
        {
            if (slot.row >= 1 && slot.row <= gridSize && slot.column >= 1 && slot.column <= gridSize)
            {
                grid[slot.row, slot.column] = slot;
            }
            else
            {
                Debug.LogWarning($"Invalid row/col for slot: {slot.name}");
            }
        }

        // Assign neighbors
        foreach (BlockSlot slot in slots)
        {
            int r = slot.row;
            int c = slot.column;

            slot.left = (c > 1) ? grid[r, c - 1] : null;
            slot.right = (c < gridSize) ? grid[r, c + 1] : null;
            slot.up = (r > 1) ? grid[r - 1, c] : null;
            slot.down = (r < gridSize) ? grid[r + 1, c] : null;
        }

        Debug.Log("BlockSlot connections assigned successfully.");
    }
}
#endif
