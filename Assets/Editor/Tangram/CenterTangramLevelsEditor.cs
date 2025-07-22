using UnityEngine;
using UnityEditor;

public class CenterTangramLevelsEditor : EditorWindow
{
    private const string basePath = "Tangram/Pack1";

    [MenuItem("Tools/Center All Tangram Levels To Grid")]
    public static void CenterAllTangramLevelsToGrid()
    {
        for (int i = 0; i < 50; i++)
        {
            string path = $"{basePath}/Level{i}";
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab not found at Resources/{path}");
                continue;
            }

            string assetPath = AssetDatabase.GetAssetPath(prefab);
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (instance == null)
            {
                Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
                continue;
            }

            CenterToTangramGrid(instance);
            PrefabUtility.SaveAsPrefabAssetAndConnect(instance, assetPath, InteractionMode.AutomatedAction);
            GameObject.DestroyImmediate(instance);
        }
        Debug.Log("✅ All Tangram levels aligned to Tangram10x10Grid center.");
    }

    private static void CenterToTangramGrid(GameObject root)
    {
        Transform grid = root.transform.Find("Tangram10x10Grid");
        if (grid == null)
        {
            Debug.LogWarning($"❌ 'Tangram10x10Grid' not found in {root.name}");
            return;
        }

        Vector3 gridWorldPos = grid.position;
        Vector3 rootWorldPos = root.transform.position;
        Vector3 offset = gridWorldPos - rootWorldPos; // Note: reversed the calculation

        // Move root to grid position first
        root.transform.position = gridWorldPos;

        // Then move all children (except the grid) back by the offset to maintain their relative positions
        foreach (Transform child in root.transform)
        {
            if (child != grid) // Don't move the grid itself
            {
                child.position -= offset;
            }
        }
    }
}