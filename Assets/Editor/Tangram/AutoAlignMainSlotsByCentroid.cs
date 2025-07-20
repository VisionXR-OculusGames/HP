#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class AutoAlignMainSlotsByCentroid : EditorWindow
{
    [MenuItem("Tools/Tangram/Fix MainTangramSlotsRoot Alignment")]
    public static void FixSlotPositions()
    {
        for (int levelIndex = 0; levelIndex <= 49; levelIndex++)
        {
            string path = $"Tangram/Pack1/Level{levelIndex}";
            GameObject levelPrefab = Resources.Load<GameObject>(path);
            if (levelPrefab == null)
            {
                Debug.LogWarning($"Level{levelIndex} not found at Resources/{path}");
                continue;
            }

            GameObject levelInstance = (GameObject)PrefabUtility.InstantiatePrefab(levelPrefab);
            if (levelInstance == null)
            {
                Debug.LogWarning($"Could not instantiate Level{levelIndex}");
                continue;
            }

            Transform mainSlotsRoot = levelInstance.transform.Find("MainTangramSlotsRoot");
            Transform gridRoot = levelInstance.transform.Find("Tangram10x10Grid");

            if (mainSlotsRoot == null || gridRoot == null)
            {
                Debug.LogWarning($"MainTangramSlotsRoot or Tangram Slot Grid not found in {levelInstance.name}");
                DestroyImmediate(levelInstance);
                continue;
            }

            foreach (Transform slotTransform in mainSlotsRoot)
            {
                TangramSlot slot = slotTransform.GetComponent<TangramSlot>();
                if (slot == null || slot.subSlots == null || slot.subSlots.Count == 0) continue;

                Vector3 centroid = Vector3.zero;
                int count = 0;

                foreach (TangramSubSlot sub in slot.subSlots)
                {
                    if (sub != null)
                    {
                        centroid += sub.transform.position;
                        count++;
                    }
                }

                if (count > 0)
                {
                    centroid /= count;
                    slotTransform.position = centroid;
                }
            }

            // Apply changes to prefab
            string prefabPath = $"Assets/Resources/Tangram/Pack1/Level{levelIndex}.prefab";
            PrefabUtility.SaveAsPrefabAsset(levelInstance, prefabPath);
            Debug.Log($"✅ Updated {levelInstance.name} slot positions and saved to prefab.");
            DestroyImmediate(levelInstance);
        }

        Debug.Log("✅ All MainTangramSlotsRoot slot positions aligned based on centroid of subslots.");
    }
}
#endif
