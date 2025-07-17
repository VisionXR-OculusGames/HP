#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class TangramSlotAndPieceMaterialUpdater : EditorWindow
{
    private Material newMaterial;

    [MenuItem("Tools/Tangram/Update Slot & Piece5 Materials")]
    public static void ShowWindow()
    {
        GetWindow<TangramSlotAndPieceMaterialUpdater>("Tangram Material Updater");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tangram Material Updater", EditorStyles.boldLabel);
        newMaterial = (Material)EditorGUILayout.ObjectField("New Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Update All Levels"))
        {
            if (newMaterial == null)
            {
                EditorUtility.DisplayDialog("Missing Material", "Please assign a material.", "OK");
                return;
            }

            UpdateAllLevels("Tangram/Pack1", 50, updatePiece5: false);  // Slot only
            UpdateAllLevels("Tangram/Pack2", 75, updatePiece5: true);   // Slot + Piece5

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("✅ All materials updated.");
        }
    }

    private void UpdateAllLevels(string folderPath, int levelCount, bool updatePiece5)
    {
        for (int i = 0; i < levelCount; i++)
        {
            string levelName = $"Level{i}";
            string resourcePath = $"{folderPath}/{levelName}";
            string assetPath = $"Assets/Resources/{folderPath}/{levelName}.prefab";

            GameObject loadedPrefab = Resources.Load<GameObject>(resourcePath);
            if (loadedPrefab == null)
            {
                Debug.LogWarning($"❌ Could not load prefab at: {resourcePath}");
                continue;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(loadedPrefab);
            bool modified = false;

            // 1️⃣ Update TangramSubSlots
      

            // 2️⃣ Update TangramSubPieces under Piece5
            if (updatePiece5)
            {
                var piece5 = instance.transform.Find("Piece5");
                if (piece5 != null)
                {
                    var subPieces = piece5.GetComponentsInChildren<TangramSubPiece>(true);
                    foreach (var piece in subPieces)
                    {
                        var mr = piece.GetComponent<MeshRenderer>();
                        if (mr != null)
                        {
                            Undo.RecordObject(mr, "Update Piece5 Material");
                            mr.sharedMaterial = newMaterial;
                            modified = true;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"⚠️ 'Piece5' not found in {resourcePath}");
                }
            }

            if (modified)
            {
                PrefabUtility.SaveAsPrefabAsset(instance, assetPath);
                Debug.Log($"✅ Updated: {resourcePath}");
            }

            DestroyImmediate(instance);
        }
    }
}
#endif
