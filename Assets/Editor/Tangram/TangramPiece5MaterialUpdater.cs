#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public class TangramPiece5MaterialUpdater : EditorWindow
{
    private Material newMaterial;

    [MenuItem("Tools/Tangram/Update Piece5 Materials")]
    public static void ShowWindow()
    {
        GetWindow<TangramPiece5MaterialUpdater>("Piece5 Material Updater");
    }

    private void OnGUI()
    {
        GUILayout.Label("Piece5 Material Updater", EditorStyles.boldLabel);
        newMaterial = (Material)EditorGUILayout.ObjectField("New Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Update Piece5 in Pack2"))
        {
            if (newMaterial == null)
            {
                EditorUtility.DisplayDialog("Missing Material", "Please assign a material first.", "OK");
                return;
            }

            UpdatePiece5InPack2();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("✅ Piece5 materials updated in all Pack2 levels.");
        }
    }

    private void UpdatePiece5InPack2()
    {
        string folderPath = "Tangram/Pack2";

        for (int i = 0; i < 75; i++)
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

            Transform piece5 = instance.transform.Find("Piece5");
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
                Debug.LogWarning($"⚠️ 'Piece5' not found in: {resourcePath}");
            }

            if (modified)
            {
                PrefabUtility.SaveAsPrefabAsset(instance, assetPath);
                Debug.Log($"✅ Updated Piece5 in: {resourcePath}");
            }

            DestroyImmediate(instance);
        }
    }
}
#endif
