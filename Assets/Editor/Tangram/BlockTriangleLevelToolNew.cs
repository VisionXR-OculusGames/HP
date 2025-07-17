using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BlockTriangleLevelToolNew : EditorWindow
{
    private GameObject trianglePrefab; // Single triangle prefab
    private GameObject parentHolder;   // Parent for organizing in hierarchy

    private int triangleCount = 9;
    private List<GameObject> triangles = new List<GameObject>();
    private bool selectionMode = false;

    [MenuItem("Tools/Triangle Grid Generator")]
    public static void ShowWindow()
    {
        GetWindow<BlockTriangleLevelToolNew>("Triangle Grid Tool");
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        GUILayout.Label("Triangle Grid Generator (Tangram Style)", EditorStyles.boldLabel);

        trianglePrefab = (GameObject)EditorGUILayout.ObjectField("Triangle Prefab", trianglePrefab, typeof(GameObject), false);
        parentHolder = (GameObject)EditorGUILayout.ObjectField("Parent Holder", parentHolder, typeof(GameObject), true);

        triangleCount = EditorGUILayout.IntSlider("Number of Triangles", triangleCount, 1, 36);

        if (GUILayout.Button("Generate Triangle Grid"))
        {
            if (trianglePrefab == null || parentHolder == null)
            {
                Debug.LogError("Please assign triangle prefab and parent.");
                return;
            }

            ClearTriangles();
            GenerateTriangleGrid();
        }

        GUILayout.Space(10);
        if (GUILayout.Button(selectionMode ? "Exit Selection Mode" : "Enter Selection Mode"))
        {
            selectionMode = !selectionMode;
            SceneView.RepaintAll();
        }

        GUI.enabled = selectionMode;
        if (GUILayout.Button("Keep Selected and Delete Others"))
        {
            ApplySelection();
        }
        GUI.enabled = true;
    }

    private void ClearTriangles()
    {
        triangles.Clear();

        for (int i = parentHolder.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parentHolder.transform.GetChild(i).gameObject);
        }
    }

    private void GenerateTriangleGrid()
    {
        float size = 0.11f;
        float xOffset = 0;
        float yOffset = 0;
        int row = 0;
        int col = 0;

        for (int i = 0; i < triangleCount; i++)
        {
            if (col >= row + 1)
            {
                row++;
                col = 0;
                xOffset = -row * size * 0.5f;
                yOffset = -row * size * 0.866f; // sin(60°) ≈ 0.866 for equilateral triangle height
            }

            Vector3 position = new Vector3(xOffset + col * size, -yOffset, 0);
            GameObject triangle = (GameObject)PrefabUtility.InstantiatePrefab(trianglePrefab);
            triangle.name = $"Triangle_{row}_{col}";
            triangle.transform.position = position;
            triangle.transform.SetParent(parentHolder.transform);
            triangle.AddComponent<BlockTriangleCell>().SetCoordinates(row, col);
            triangles.Add(triangle);
            col++;
        }

        Debug.Log($"Generated {triangleCount} triangles in pyramid layout.");
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (!selectionMode) return;

        Event e = Event.current;
        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                var cell = hit.collider.GetComponentInParent<BlockTriangleCell>();
                if (cell != null)
                {
                    cell.isSelected = !cell.isSelected;
                    var rend = cell.GetComponentInChildren<Renderer>();
                    if (rend != null)
                    {
                        rend.material.color = cell.isSelected ? Color.green : Color.white;
                    }
                    e.Use();
                }
            }
        }
    }

    private void ApplySelection()
    {
        List<GameObject> toKeep = new List<GameObject>();

        foreach (GameObject triangle in triangles)
        {
            BlockTriangleCell cell = triangle.GetComponent<BlockTriangleCell>();
            if (cell != null && cell.isSelected)
            {
                toKeep.Add(triangle);
            }
            else
            {
                DestroyImmediate(triangle);
            }
        }

        triangles = toKeep;
        selectionMode = false;
        Debug.Log("Selection applied. Unselected triangles removed.");
    }
}

public class BlockTriangleCell : MonoBehaviour
{
    public int row;
    public int col;
    public bool isSelected = false;

    public void SetCoordinates(int r, int c)
    {
        row = r;
        col = c;
    }
}
