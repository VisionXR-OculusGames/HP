using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MatchStick2Generator : EditorWindow
{
    [Header("Prefab Data")]
    public List<GameObject> digitPrefabs = new List<GameObject>();     // 0-9
    public List<GameObject> symbolPrefabs = new List<GameObject>();    // + - * / =

    private string number1 = "12";
    private int operator1Index = 0;
    private string number2 = "3";
    private int operator2Index = 4; // default '='
    private string number3 = "27";

    private Transform levelParent;

    [Header("Spacing Settings")]
    public float xOffset = 1.2f;         // space between number/symbol blocks
    public float numberOffset = 0.6f;    // space between digits in a number

    private readonly string[] symbolOptions = { "+", "-", "×", "÷", "=" };

    [MenuItem("Tools/Matchstick 2 Generator")]
    public static void ShowWindow()
    {
        GetWindow<MatchStick2Generator>("Matchstick 2 Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Matchstick Equation Generator (Advanced)", EditorStyles.boldLabel);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty digitsProp = so.FindProperty("digitPrefabs");
        SerializedProperty symbolsProp = so.FindProperty("symbolPrefabs");
        EditorGUILayout.PropertyField(digitsProp, new GUIContent("Digit Prefabs (0-9)"), true);
        EditorGUILayout.PropertyField(symbolsProp, new GUIContent("Symbol Prefabs (+ - * / =)"), true);
        so.ApplyModifiedProperties();

        EditorGUILayout.Space();
        number1 = EditorGUILayout.TextField("Number 1", number1);
        operator1Index = EditorGUILayout.Popup("Operator 1", operator1Index, symbolOptions);
        number2 = EditorGUILayout.TextField("Number 2", number2);
        operator2Index = EditorGUILayout.Popup("Operator 2", operator2Index, symbolOptions);
        number3 = EditorGUILayout.TextField("Number 3", number3);

        EditorGUILayout.Space();
        xOffset = EditorGUILayout.FloatField("X Offset", xOffset);
        numberOffset = EditorGUILayout.FloatField("Number Offset", numberOffset);
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

        float currentX = 0f;

        // Helper to spawn a number string
        void SpawnNumber(string num)
        {
            foreach (char c in num)
            {
                if (char.IsDigit(c))
                {
                    int digit = c - '0';
                    if (digit >= 0 && digit < digitPrefabs.Count)
                    {
                        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(digitPrefabs[digit], levelParent);
                        instance.transform.localPosition = new Vector3(currentX, 0, 0);
                        currentX += numberOffset;
                    }
                }
            }
            currentX += xOffset - numberOffset; // compensate after number group
        }

        // Helper to spawn symbol
        void SpawnSymbol(int index)
        {
            if (index >= 0 && index < symbolPrefabs.Count)
            {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(symbolPrefabs[index], levelParent);
                instance.transform.localPosition = new Vector3(currentX, 0, 0);
                currentX += xOffset;
            }
        }

        // Generate full equation
        SpawnNumber(number1);
        SpawnSymbol(operator1Index);
        SpawnNumber(number2);
        SpawnSymbol(operator2Index);
        SpawnNumber(number3);

        Debug.Log("? Advanced matchstick equation generated.");
    }
}
