using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MatchStickGenerator3 : EditorWindow
{
    [Header("Prefab Data")]
    public List<GameObject> digitPrefabs = new List<GameObject>();     // 0-9
    public List<GameObject> symbolPrefabs = new List<GameObject>();    // + - * / =

    private string number1 = "3";
    private int operator1Index = 0;
    private string number2 = "3";
    private int operator2Index = 4; // default '='
    private string number3 = "6";
    private int operator3Index = 0;
    private string number4 = "6";

    private Transform levelParent;

    [Header("Spacing Settings")]
    public float xOffset = 1.2f;
    public float numberOffset = 0.6f;

    private readonly string[] symbolOptions = { "+", "-", "×", "÷", "=" };

    [MenuItem("Tools/Matchstick 3 Generator")]
    public static void ShowWindow()
    {
        GetWindow<MatchStickGenerator3>("Matchstick 3 Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Matchstick Equation Generator (4 numbers, 3 operators)", EditorStyles.boldLabel);

        SerializedObject so = new SerializedObject(this);
        EditorGUILayout.PropertyField(so.FindProperty("digitPrefabs"), new GUIContent("Digit Prefabs (0–9)"), true);
        EditorGUILayout.PropertyField(so.FindProperty("symbolPrefabs"), new GUIContent("Symbol Prefabs (+ - * / =)"), true);
        so.ApplyModifiedProperties();

        EditorGUILayout.Space();
        number1 = EditorGUILayout.TextField("Number 1", number1);
        operator1Index = EditorGUILayout.Popup("Operator 1", operator1Index, symbolOptions);
        number2 = EditorGUILayout.TextField("Number 2", number2);
        operator2Index = EditorGUILayout.Popup("Operator 2", operator2Index, symbolOptions);
        number3 = EditorGUILayout.TextField("Number 3", number3);
        operator3Index = EditorGUILayout.Popup("Operator 3", operator3Index, symbolOptions);
        number4 = EditorGUILayout.TextField("Number 4", number4);

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
            currentX += xOffset - numberOffset;
        }

        void SpawnSymbol(int index)
        {
            if (index >= 0 && index < symbolPrefabs.Count)
            {
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(symbolPrefabs[index], levelParent);
                instance.transform.localPosition = new Vector3(currentX, 0, 0);
                currentX += xOffset;
            }
        }

        // Build: number1 op1 number2 op2 number3 op3 number4
        SpawnNumber(number1);
        SpawnSymbol(operator1Index);
        SpawnNumber(number2);
        SpawnSymbol(operator2Index);
        SpawnNumber(number3);
        SpawnSymbol(operator3Index);
        SpawnNumber(number4);

        Debug.Log("? Matchstick equation with 4 numbers generated.");
    }
}
