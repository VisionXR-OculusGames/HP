using com.VisionXR.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTangramLevel : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public AudioDataSO audioData;

    [Header("Gameplay Objects")]
    public List<TangramSlot> targetSlots;
    public List<TangramSlot> initialSlots;
    public List<GameObject> pieces;

    [Header("Solution Setup")]
    public List<TangramSolution> solutions;

    [Header(" Display Data")]
    public int totalMoves = 0;
    public int levelNo;
    private int moves = 0;


    private int currentMoves = 0;

    private List<GameObject> selectedPieces = new();
    private List<Vector3> initPositions = new();
    private List<Quaternion> initRotations = new();
    private TangramSlot closestSlot;

    [SerializeField] private float lerpDuration = 0.5f;
    private BoxCollider triggerBox;

    private Dictionary<GameObject, TangramSlot> initialSlotMap = new();

    //local variables
    private bool isMoved = false;

    private void OnEnable()
    {
        inputData.PinchStartEvent += StartPinch;
        inputData.PinchEndEvent += EndPinch;
        inputData.PinchContinueEvent += ContinuePinch;

        triggerBox = GetComponent<BoxCollider>();
        Initialize();

    }

    private void Initialize()
    {
        currentMoves = 0;
        moves = 0;
        levelData.SetTangramMoves(moves, totalMoves);
        levelData.SetLevel(levelNo);
        InitializeInitialSlotMap();
    }

    private void OnDisable()
    {
        inputData.PinchStartEvent -= StartPinch;
        inputData.PinchEndEvent -= EndPinch;
        inputData.PinchContinueEvent -= ContinuePinch;
    }
    public bool IsPositionInsideBox(Vector3 worldPos)
    {
        if (triggerBox == null) return true;

        return triggerBox.bounds.Contains(worldPos);
    }
    private void InitializeInitialSlotMap()
    {
        for (int i = 0; i < Mathf.Min(pieces.Count, initialSlots.Count); i++)
        {
            initialSlotMap[pieces[i]] = initialSlots[i];
        }
    }

    private void StartPinch(Vector3 pos, Quaternion rot)
    {
        if (!IsPositionInsideBox(pos))
            return;

        GameObject picked = GetClosestPiece(pos);
        if (picked != null)
        {
            selectedPieces.Add(picked);
            initPositions.Add(picked.transform.position);
            initRotations.Add(picked.transform.rotation);
            UnmarkSlot(picked);
            audioData.PlayGrabSound();
        }
        isMoved = false;

    }


    private void ContinuePinch(Vector3 pos, Quaternion rot)
    {
        if (selectedPieces.Count == 0) return;
        GameObject current = selectedPieces[^1];
        current.transform.position = pos;
        current.transform.rotation = rot;

        if (closestSlot != null) closestSlot.ResetGlow();
        closestSlot = GetClosestSlot(current);
        if (closestSlot != null && !closestSlot.isOccupied)
            closestSlot.SetGlow();
            
    }

    private void EndPinch(Vector3 pos, Quaternion rot)
    {
        if (selectedPieces.Count == 0) return;

        audioData.PlayUnGrabSound();
        GameObject current = selectedPieces[^1];
        TangramSlot match = GetClosestSlot(current);
        if (closestSlot != null) closestSlot.ResetGlow();

        if (match != null && !match.isOccupied)
        {
            SnapToSlot(current, match);
            currentMoves++;
        }
        else
        {
            ResetPiece(current, initPositions[^1], initRotations[^1]);
        }

        if (IsLevelComplete())
        {
            audioData.PlayLevelCompletedSound();
            levelData.TangramLevelSuccess();
        }
        if (isMoved)
        {
            moves += 1;
            levelData.SetTangramMoves(moves, totalMoves);
        }
    }

    private GameObject GetClosestPiece(Vector3 pos)
    {
        float minDist = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject piece in pieces)
        {
            float dist = Vector3.Distance(piece.transform.position, pos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = piece;
            }
        }
        return closest;
    }

    private TangramSlot GetClosestSlot(GameObject piece)
    {
        float minDist = float.MaxValue;
        TangramSlot closest = null;

        var subPieces = piece.GetComponentsInChildren<TangramSubPiece>();
        if (subPieces.Length == 0) return null;

        string pieceTag = subPieces[0].shapeTag;

        foreach (TangramSlot slot in targetSlots)
        {
            if (slot.isOccupied) continue;
            if (slot.subSlots.Count != subPieces.Length) continue;
            if (slot.shapeTag != pieceTag) continue;

            float dist = Vector3.Distance(slot.transform.position, piece.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = slot;
                isMoved = true;
            }
        }

        if (initialSlotMap.TryGetValue(piece, out TangramSlot allowedInitial))
        {
            if (!allowedInitial.isOccupied &&
                allowedInitial.subSlots.Count == subPieces.Length &&
                allowedInitial.shapeTag == pieceTag)
            {
                float dist = Vector3.Distance(allowedInitial.transform.position, piece.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = allowedInitial;
                    isMoved = false;
                }
            }
        }
        return closest;
    }




    private void SnapToSlot(GameObject piece, TangramSlot slot)
    {
        var subPieces = piece.GetComponentsInChildren<TangramSubPiece>();
        if (subPieces.Length != slot.subSlots.Count)
        {
            return;
        }

        for (int i = 0; i < subPieces.Length; i++)
        {
            slot.subSlots[i].isOccupied = true;
            slot.subSlots[i].occupiedBy = subPieces[i].gameObject;
            subPieces[i].matchedSlot = slot.subSlots[i];
        }

        slot.occupiedBy = piece;
        piece.transform.position = slot.transform.position + new Vector3(0,0,-0.005f);
        piece.transform.rotation = slot.transform.rotation;
        selectedPieces.Clear();
        initPositions.Clear();
        initRotations.Clear();
    }

    private void ResetPiece(GameObject piece, Vector3 pos, Quaternion rot)
    {
        StartCoroutine(LerpPiece(piece, pos, rot));
    }

    private IEnumerator LerpPiece(GameObject piece, Vector3 pos, Quaternion rot)
    {
        float elapsed = 0f;
        Vector3 startPos = piece.transform.position;
        Quaternion startRot = piece.transform.rotation;

        while (elapsed < lerpDuration)
        {
            float t = elapsed / lerpDuration;
            piece.transform.position = Vector3.Lerp(startPos, pos, t);
            piece.transform.rotation = Quaternion.Slerp(startRot, rot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        piece.transform.position = pos;
        piece.transform.rotation = rot;

        selectedPieces.Clear();
        initPositions.Clear();
        initRotations.Clear();
    }

    private bool IsLevelComplete()
    {
        foreach (var sol in solutions)
        {
            if (sol.correctSlot.occupiedBy != sol.correctPiece)
                return false;
        }
        return true;
    }

    private void UnmarkSlot(GameObject piece)
    {

        List<TangramSlot> allSlots = new();
        allSlots.AddRange(targetSlots);
        allSlots.AddRange(initialSlots);

        foreach (var slot in allSlots)
        {
            foreach (var sub in slot.subSlots)
            {

                if (sub.occupiedBy != null && sub.occupiedBy.transform.IsChildOf(piece.transform)) 
                {
                    sub.isOccupied = false;
                    sub.occupiedBy = null;
                }
            }

            if (slot.occupiedBy == piece)
            {
                slot.occupiedBy = null;
            }
        }
    }




}

[System.Serializable]
public class TangramSolution
{
    public GameObject correctPiece;
    public TangramSlot correctSlot;
}
