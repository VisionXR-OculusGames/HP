#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TangramPiece5TriangleMaterialUpdater : EditorWindow
{
    private Material newMaterial;

    [MenuItem("Tools/Tangram/Update Piece5 Triangle Materials")]
    public static void ShowWindow()
    {
        GetWindow<TangramPiece5TriangleMaterialUpdater>("Piece5 Triangle Material Updater");
    }

    private void OnGUI()
    {
        GUILayout.Label("Piece5 Triangle Material Updater", EditorStyles.boldLabel);
        newMaterial = (Material)EditorGUILayout.ObjectField("New Material", newMaterial, typeof(Material), false);

        if (GUILayout.Button("Update Piece5 Triangles in Pack2"))
        {
            if (newMaterial == null)
            {
                EditorUtility.DisplayDialog("Missing Material", "Please assign a material first.", "OK");
                return;
            }

            UpdateTrianglesInPack2();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("✅ Finished updating Piece5 triangle materials.");
        }
    }

    private void UpdateTrianglesInPack2()
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

            Transform missingPieces = instance.transform.Find("MissingPieces");
            if (missingPieces != null)
            {
                Transform piece5 = missingPieces.Find("Piece5");
                if (piece5 != null)
                {
                    foreach (Transform triangle in piece5)
                    {
                        if (triangle.GetComponent<TangramSubPiece>() != null)
                        {
                            MeshRenderer mr = triangle.GetComponent<MeshRenderer>();
                            if (mr != null)
                            {
                                Undo.RecordObject(mr, "Update Triangle Material");
                                mr.sharedMaterial = newMaterial;
                                modified = true;
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"⚠️ 'Piece5' not found in: {resourcePath}");
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ 'MissingPieces' not found in: {resourcePath}");
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
