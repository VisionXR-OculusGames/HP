using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MatchStickLevelGenerator : EditorWindow
{
    [Header("Prefab Data")]
    public List<GameObject> prefabList = new List<GameObject>();
    public List<string> prefabNames = new List<string>();

    private int[] selectedIndices = new int[5];
    private Transform levelParent;

    [Header("Spacing Settings")]
    public float xOffset = 1.2f; // now editable

    [MenuItem("Tools/Matchstick Level Generator")]
    public static void ShowWindow()
    {
        GetWindow<MatchStickLevelGenerator>("Matchstick Level Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Matchstick Equation Builder", EditorStyles.boldLabel);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty prefabListProp = so.FindProperty("prefabList");
        EditorGUILayout.PropertyField(prefabListProp, new GUIContent("Prefab List"), true);
        so.ApplyModifiedProperties();

        // Refresh prefab names
        prefabNames.Clear();
        foreach (GameObject go in prefabList)
        {
            prefabNames.Add(go != null ? go.name : "Null");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Build Equation:");

        for (int i = 0; i < selectedIndices.Length; i++)
        {
            selectedIndices[i] = EditorGUILayout.Popup($"Slot {i + 1}", selectedIndices[i], prefabNames.ToArray());
        }

        EditorGUILayout.Space();
        xOffset = EditorGUILayout.FloatField("X Offset", xOffset);
        levelParent = (Transform)EditorGUILayout.ObjectField("Level Parent", levelParent, typeof(Transform), true);

        if (GUILayout.Button("Generate Level"))
        {
            GenerateLevel();
        }
    }

    private void GenerateLevel()
    {
        if (levelParent == null)
        {
            Debug.LogWarning("?? Please assign a Level Parent.");
            return;
        }

        float startX = -(xOffset * (selectedIndices.Length - 1)) / 2f;

        for (int i = 0; i < selectedIndices.Length; i++)
        {
            GameObject prefab = prefabList[selectedIndices[i]];
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab at index {selectedIndices[i]} is null.");
                continue;
            }

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, levelParent);
            instance.transform.localPosition = new Vector3(startX + i * xOffset, 0, 0);
        }

        Debug.Log("? Level generated.");
    }
}
