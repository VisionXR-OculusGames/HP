using UnityEditor;
using UnityEngine;
using System.IO;

public class AddColliderToTangramPrefabs
{
    [MenuItem("Tools/Tangram/Add BoxCollider To All Prefabs")]
    public static void AddBoxColliderToAllPrefabs()
    {
        string basePath = "Tangram/Pack2/Level";
        int totalPrefabs = 75;

        for (int i = 0; i < totalPrefabs; i++)
        {
            string fullPath = basePath + i;
            Debug.Log(fullPath);
            GameObject prefab = Resources.Load<GameObject>(fullPath); ;

            if (prefab == null)
            {
                Debug.LogWarning("Prefab not found at: " + fullPath);
                continue;
            }

            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            if (prefabInstance != null)
            {
                // Add BoxCollider if not already present
                if (prefabInstance.GetComponent<BoxCollider>() == null)
                {
                    var box = prefabInstance.AddComponent<BoxCollider>();
                    box.size = new Vector3(1.87f, 1.23f, 0.13f); // Customize to your gameplay grid size
                    box.center = new Vector3(0.08f, -0.4f, 0); // Adjust as needed
                    Debug.Log($"BoxCollider added to Level{i}");
                }

                // Apply changes back to prefab
                PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                GameObject.DestroyImmediate(prefabInstance);
            }
        }

        Debug.Log("✅ All Tangram prefabs processed.");
    }
}
