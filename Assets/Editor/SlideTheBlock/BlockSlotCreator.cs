using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

public class BlockSlotCreator : EditorWindow
{
    private int gridSize = 4;
    private GameObject slotPrefab;
    private GameObject parentObject;

    [MenuItem("Tools/Block Slot Creator")]
    public static void ShowWindow()
    {
        GetWindow<BlockSlotCreator>("Block Slot Creator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Grid of Slots", EditorStyles.boldLabel);

        gridSize = EditorGUILayout.IntSlider("Grid Size (n)", gridSize, 2, 10);
        slotPrefab = (GameObject)EditorGUILayout.ObjectField("Slot Prefab", slotPrefab, typeof(GameObject), false);
        parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);

        if (GUILayout.Button("Create Slots"))
        {
            if (slotPrefab == null)
            {
                Debug.LogError("Slot prefab must be assigned!");
                return;
            }

            if (parentObject == null)
            {
                Debug.LogError("Parent object must be assigned!");
                return;
            }

            CreateSlots();
        }
    }

    private void CreateSlots()
    {
        float unit = 1f / gridSize;
        float halfunit = unit/2;

        for (int row = 1; row <= gridSize; row++)
        {
            for (int col = 1; col <= gridSize; col++)
            {
                float x = -(unit + (gridSize - 3) * halfunit) + (col - 1) * unit;
                float y = (unit+ (gridSize-3)*halfunit)-(row-1)*unit;
                Vector3 pos = new Vector3(x, y, 0);

                GameObject slot = (GameObject)PrefabUtility.InstantiatePrefab(slotPrefab);
                slot.name = $"Slot_{row}_{col}";
                slot.transform.position = pos;
                slot.transform.SetParent(parentObject.transform);

                BlockSlot bSlot = slot.AddComponent<BlockSlot>();
                bSlot.row = row;
                bSlot.column = col;
               
            }
        }

        Debug.Log($"Created {gridSize * gridSize} centered slots under {parentObject.name}.");
    }
}
