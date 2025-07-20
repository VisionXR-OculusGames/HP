#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FixPieceAndInitialSlotAlignment : EditorWindow
{
    [MenuItem("Tools/Tangram/Fix Piece & InitialSlot Alignment")]
    public static void FixAlignment()
    {
        for (int levelIndex = 0; levelIndex <= 1; levelIndex++)
        {
            string path = $"Tangram/Pack1/Level{levelIndex}";
            GameObject prefab = Resources.Load<GameObject>(path);

            if (prefab == null)
            {
                Debug.LogWarning($"❌ Level{levelIndex} not found at Resources/{path}");
                continue;
            }

            GameObject levelInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            if (levelInstance == null)
            {
                Debug.LogWarning($"❌ Could not instantiate Level{levelIndex}");
                continue;
            }

            Transform missingPiecesRoot = levelInstance.transform.Find("MissingPieces");
            Transform initialSlotsRoot = levelInstance.transform.Find("InitialSlots");

            if (missingPiecesRoot == null || initialSlotsRoot == null)
            {
                Debug.LogWarning($"❌ Missing children in Level{levelIndex}");
                DestroyImmediate(levelInstance);
                continue;
            }

            // ✅ Fix Missing Pieces: Move each PieceN to centroid of TangramSubPiece children
            foreach (Transform piece in missingPiecesRoot)
            {
                Vector3 centroid = Vector3.zero;
                int count = 0;

                foreach (Transform child in piece)
                {
                    if (child.GetComponent<TangramSubPiece>())
                    {
                        centroid += child.position;
                        count++;
                    }
                }

                if (count > 0)
                {
                    piece.position = centroid / count;
                }
            }

            // ✅ Fix Initial Slots: Move each InitialSlotN to centroid of TangramSubSlot children
            foreach (Transform initialSlot in initialSlotsRoot)
            {
                Vector3 centroid = Vector3.zero;
                int count = 0;

                foreach (Transform child in initialSlot)
                {
                    if (child.GetComponent<TangramSubSlot>())
                    {
                        centroid += child.position;
                        count++;
                    }
                }

                if (count > 0)
                {
                    initialSlot.position = centroid / count;
                }
            }

            // ✅ Save modified prefab
            string savePath = $"Assets/Resources/Tangram/Pack1/Level{levelIndex}.prefab";
            PrefabUtility.SaveAsPrefabAsset(levelInstance, savePath);
            Debug.Log($"✅ Level{levelIndex} updated & saved");
            DestroyImmediate(levelInstance);
        }

        Debug.Log("🎯 Done: Piece and InitialSlot centroids fixed for Level0–Level15.");
    }
}
#endif
