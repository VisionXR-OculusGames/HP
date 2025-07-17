#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class TangramSlotMaterialUpdater : EditorWindow
{
    private Material newMaterial;

    [MenuItem("Tools/Tangram/Update Slot Materials")]
    public static void ShowWindow()
    {
        GetWindow<TangramSlotMaterialUpdater>("Tangram Slot Material Updater");
    }

    private void OnGUI()
    {
        GUILayout.Label("Slot Material Updater", EditorStyles.boldLabel);
        newMaterial = (Material)EditorGUILayout.ObjectField("New Slot Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Update All Levels"))
        {
            if (newMaterial == null)
            {
                EditorUtility.DisplayDialog("Missing Material", "Please assign a material first.", "OK");
                return;
            }

            UpdateAllLevels("Tangram/Pack1", 50);  // Level0 to Level49
            UpdateAllLevels("Tangram/Pack2", 75);  // Level0 to Level74

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("✅ Slot materials updated successfully.");
        }
    }

    private void UpdateAllLevels(string folderPath, int levelCount)
    {
        for (int i = 0; i < levelCount; i++)
        {
            string levelName = $"Level{i}";
            string fullPath = Path.Combine("Resources", folderPath, levelName);

            GameObject loadedPrefab = Resources.Load<GameObject>($"{folderPath}/{levelName}");
            if (loadedPrefab == null)
            {
                Debug.LogWarning($"❌ Could not load prefab at: {folderPath}/{levelName}");
                continue;
            }

            // Instantiate, modify, and apply back to prefab
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(loadedPrefab);
            bool modified = false;

            var subSlots = instance.GetComponentsInChildren<TangramSubSlot>(true);
            foreach (var subSlot in subSlots)
            {
                var mr = subSlot.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    Undo.RecordObject(mr, "Update Slot Material");
                    mr.sharedMaterial = newMaterial;
                    modified = true;
                }
            }

            if (modified)
            {
                PrefabUtility.SaveAsPrefabAsset(instance, $"Assets/Resources/{folderPath}/{levelName}.prefab");
                Debug.Log($"✅ Updated materials in: {folderPath}/{levelName}");
            }

            DestroyImmediate(instance);
        }
    }
}
#endif
