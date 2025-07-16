using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MatchStickLevelSetter : EditorWindow
{
    public GameObject levelParent;  // GameObject with MatchStickLevel script
    public List<GameObject> childrenContainers = new List<GameObject>();  // where sticks and spots are

    [MenuItem("Tools/Matchstick Level Setter")]
    public static void ShowWindow()
    {
        GetWindow<MatchStickLevelSetter>("Matchstick Level Setter");
    }

    private void OnGUI()
    {
        levelParent = (GameObject)EditorGUILayout.ObjectField("Level Parent", levelParent, typeof(GameObject), true);

        SerializedObject so = new SerializedObject(this);
        SerializedProperty listProp = so.FindProperty("childrenContainers");
        EditorGUILayout.PropertyField(listProp, new GUIContent("Containers (Matchsticks + Spots)"), true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Set Matchstick Level"))
        {
            SetLevelData();
        }
    }

    private void SetLevelData()
    {
        if (levelParent == null)
        {
            Debug.LogWarning("?? Assign a Level Parent.");
            return;
        }

        GenericMatchStickLevel level = levelParent.GetComponent<GenericMatchStickLevel>();
        if (level == null)
        {
            Debug.LogWarning("? No MatchStickLevel component found on Level Parent.");
            return;
        }

        List<GameObject> matchSticks = new List<GameObject>();
        List<Spot> allSpots = new List<Spot>();

        foreach (GameObject container in childrenContainers)
        {
            if (container == null) continue;

            foreach (Transform child in container.transform)
            {
                if (child.CompareTag("MatchStick"))
                {
                    MatchStick ms = child.GetComponent<MatchStick>();
                    if (ms != null && ms.type == MatchStick.MatchStickType.Movable)
                    {
                        matchSticks.Add(child.gameObject);
                    }
                }

                Spot spot = child.GetComponent<Spot>();
                if (spot != null)
                {
                    allSpots.Add(spot);
                }
            }
        }

        level.matchSticks = matchSticks;
        level.slots = allSpots;

       
        EditorUtility.SetDirty(level);
        Debug.Log("? MatchStickLevel updated.");
    }
}
