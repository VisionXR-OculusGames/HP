// TangramEditorTools.cs
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TangramEditorTools : EditorWindow
{
    private GameObject gridRoot;
    private GameObject missingPiecesRoot;
    private GameObject levelRoot;
    private GameObject initialSlotsRoot;
    private List<GameObject> emptySpots = new();

    private List<GameObject> selectedPieces = new();
    private int emptySpotIndex = 0; // place this as a private field in your class

    private float distanceFromCenter = 0.5f;  // Adjustable in inspector


    [MenuItem("Tools/Tangram Level Editor Tools")]
    public static void ShowWindow()
    {
        GetWindow<TangramEditorTools>("Tangram Level Editor");
    }

    private void OnGUI()
    {
        gridRoot = (GameObject)EditorGUILayout.ObjectField("Grid Root (TangramLevelN)", gridRoot, typeof(GameObject), true);
        missingPiecesRoot = (GameObject)EditorGUILayout.ObjectField("Missing Pieces Root", missingPiecesRoot, typeof(GameObject), true);
        levelRoot = (GameObject)EditorGUILayout.ObjectField("Level Root", levelRoot, typeof(GameObject), true);
        initialSlotsRoot = (GameObject)EditorGUILayout.ObjectField("Initial Slots Root", initialSlotsRoot, typeof(GameObject), true);

        EditorGUILayout.LabelField("Assign Empty Spots List");
        int newSize = EditorGUILayout.IntField("Size", emptySpots.Count);
        while (newSize > emptySpots.Count)
            emptySpots.Add(null);
        while (newSize < emptySpots.Count)
            emptySpots.RemoveAt(emptySpots.Count - 1);

        for (int i = 0; i < emptySpots.Count; i++)
        {
            emptySpots[i] = (GameObject)EditorGUILayout.ObjectField($"Spot {i + 1}", emptySpots[i], typeof(GameObject), true);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Destroy Unselected TangramSubPieces"))
            DestroyUnselectedSubPieces();

        if (GUILayout.Button("Destroy Unselected TangramSubSlots"))
            DestroyUnselectedSubSlots();

        if (GUILayout.Button("Step 2: Add Selection to New Piece At Empty Spots"))
            AddSelectionToNewPiece();

        if (GUILayout.Button("Step 3: Create Slot from Selection with Centered Position"))
            CreateSlotFromSelection();

        if (GUILayout.Button("Step 4: Delete Triangles Only From Tangram Shape"))
            DeleteTrianglesOnly();

        if (GUILayout.Button("🔁 Reset Empty Spot Index"))
            emptySpotIndex = 0;

        distanceFromCenter = EditorGUILayout.FloatField("Distance From Center", distanceFromCenter);
        if (GUILayout.Button("🌟 Arrange Pieces & Slots Around Grid Root"))
        {
            ArrangePiecesAndSlotsInCircle();
        }

    }
    private void ArrangePiecesAndSlotsInCircle()
    {
        if (gridRoot == null || missingPiecesRoot == null || initialSlotsRoot == null)
        {
            Debug.LogWarning("Please assign Grid Root, Missing Pieces Root, and Initial Slots Root.");
            return;
        }

        Vector3 center = gridRoot.transform.position;

        List<Transform> allObjects1 = new List<Transform>();
        List<Transform> allObjects2 = new List<Transform>();
        foreach (Transform t in missingPiecesRoot.transform)
            allObjects1.Add(t);

        foreach (Transform t in initialSlotsRoot.transform)
            allObjects2.Add(t);

        int count = allObjects1.Count;
        if (count == 0)
        {
            Debug.LogWarning("No pieces or initial slots found to arrange.");
            return;
        }

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angleRad = angleStep * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * distanceFromCenter;
            allObjects1[i].position = center + offset;
            allObjects2[i].position = center + offset;
        }

        Debug.Log($"Arranged {count} objects around grid center at radius {distanceFromCenter}");
    }

    private void DestroyUnselectedSubPieces()
    {
        var selectedSet = new HashSet<GameObject>(Selection.gameObjects);

        foreach (Transform child in gridRoot.transform)
        {
            TangramSubPiece subPiece = child.GetComponent<TangramSubPiece>();
            if (subPiece != null && !selectedSet.Contains(child.gameObject))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }

    private void DestroyUnselectedSubSlots()
    {
        var selectedSet = new HashSet<GameObject>(Selection.gameObjects);

        foreach (Transform child in gridRoot.transform)
        {
            TangramSubSlot subSlot = child.GetComponent<TangramSubSlot>();
            if (subSlot != null && !selectedSet.Contains(child.gameObject))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }




    private void AddSelectionToNewPiece()
    {
        // Clean up null references
        emptySpots.RemoveAll(spot => spot == null);
        Debug.Log($"Empty spots count: {emptySpots.Count}");
        Debug.Log($"Empty spot index: {emptySpotIndex}");

        if (emptySpotIndex >= emptySpots.Count || emptySpots[emptySpotIndex] == null)
        {
            Debug.LogWarning("No empty spot available or all spots filled.");
            return;
        }

        int pieceIndex = missingPiecesRoot.transform.childCount + 1;

        // Find centroid for subpieces and subslots
        var selected = Selection.gameObjects;
        var subPieces = selected.Select(x => x.GetComponent<TangramSubPiece>())
                                .Where(x => x != null)
                                .Select(x => x.transform.position).ToList();

        var subSlots = selected.Select(x => x.GetComponent<TangramSubSlot>())
                               .Where(x => x != null)
                               .Select(x => x.transform.position).ToList();

        Vector3 pieceCentroid = subPieces.Count > 0 ? subPieces.Aggregate(Vector3.zero, (a, b) => a + b) / subPieces.Count : Vector3.zero;
        Vector3 slotCentroid = subSlots.Count > 0 ? subSlots.Aggregate(Vector3.zero, (a, b) => a + b) / subSlots.Count : Vector3.zero;

        // Use only the current empty spot
        GameObject spot = emptySpots[emptySpotIndex];
        Vector3 offsetPiece = spot.transform.position - pieceCentroid;
        Vector3 offsetSlot = spot.transform.position - slotCentroid;

        GameObject newPiece = new GameObject("Piece" + pieceIndex);
        newPiece.transform.parent = missingPiecesRoot.transform;
        newPiece.transform.position = spot.transform.position;

        GameObject newSlotHolder = new GameObject("InitialSlot" + pieceIndex);
        newSlotHolder.transform.parent = initialSlotsRoot.transform;
        newSlotHolder.transform.position = spot.transform.position;

        TangramSlot slotComp = newSlotHolder.AddComponent<TangramSlot>();
        slotComp.subSlots = new List<TangramSubSlot>();
        slotComp.shapeTag = "Piece" + pieceIndex;

        foreach (GameObject obj in selected)
        {
            var piece = obj.GetComponent<TangramSubPiece>();
            if (piece != null)
            {
                GameObject pieceCopy = Instantiate(obj, newPiece.transform);
                Vector3 adjustedPos = obj.transform.position + offsetPiece;
                adjustedPos.z -= 0.01f; // Bring piece forward
                pieceCopy.transform.position = adjustedPos;
                pieceCopy.transform.rotation = obj.transform.rotation;

                TangramSubPiece taggable = pieceCopy.GetComponent<TangramSubPiece>();
                if (taggable != null)
                    taggable.shapeTag = "Piece" + pieceIndex;
            }


            var slot = obj.GetComponent<TangramSubSlot>();
            if (slot != null)
            {
                GameObject slotCopy = Instantiate(obj, newSlotHolder.transform);
                slotCopy.transform.position = obj.transform.position + offsetSlot;
                slotCopy.transform.rotation = obj.transform.rotation;

                TangramSubSlot refSlot = slotCopy.GetComponent<TangramSubSlot>();
                if (refSlot != null)
                    slotComp.subSlots.Add(refSlot);
            }
        }

        // Move to the next empty spot for the next call
        emptySpotIndex++;
    }


    private void CreateSlotFromSelection()
    {
        Vector3 centroid = Vector3.zero;
        List<TangramSubSlot> validSubs = new();

        foreach (GameObject obj in Selection.gameObjects)
        {
            TangramSubSlot sub = obj.GetComponent<TangramSubSlot>();
            if (sub != null)
            {
                centroid += sub.transform.position;
                validSubs.Add(sub);
            }
        }

        if (validSubs.Count == 0) return;

        centroid /= validSubs.Count;

        GameObject slotObj = new GameObject("Slot" + (levelRoot.transform.childCount + 1));
        slotObj.transform.SetParent(levelRoot.transform);
        slotObj.transform.position = centroid;

        TangramSlot slot = slotObj.AddComponent<TangramSlot>();
        slot.subSlots = validSubs;
    }

    private void DeleteTrianglesOnly()
    {
        foreach (Transform child in gridRoot.transform)
        {
            TangramSubPiece piece = child.GetComponent<TangramSubPiece>();
            if (piece != null)
            {
                DestroyImmediate(piece.gameObject);
            }
        }
    }
}
#endif
