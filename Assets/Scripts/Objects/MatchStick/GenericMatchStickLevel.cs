using com.VisionXR.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenericMatchStickLevel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public AudioDataSO audioData;

    [Header(" Game Objects")]
    public List<Spot> slots;
    public List<GameObject> matchSticks;

    [Header(" Solution Paths ")]
    public List<MatchStickSolution> solutionPaths;
    public int totalMoves = 0;
    public string hint;
    private BoxCollider triggerBox;

    // local variables

    [SerializeField] private float lerpDuration = 0.5f;
    private List<GameObject> selectedSticks = new();
    private List<Vector3> initPositions = new();
    private List<Quaternion> initRotations = new();
    private Spot closestSpot;
    private int currentMoves = 0;
  

    private void OnEnable()
    {
        inputData.PinchStartEvent += StartPinch;
        inputData.PinchEndEvent += EndPinch;
        inputData.PinchContinueEvent += ContinuePinch;
        Initailise();
    }

    private void OnDisable()
    {
        inputData.PinchStartEvent -= StartPinch;
        inputData.PinchEndEvent -= EndPinch;
        inputData.PinchContinueEvent -= ContinuePinch;
    }


    private void Initailise()
    {
        triggerBox = GetComponent<BoxCollider>();
        currentMoves = 0;
        UpdateMoveText();
        
    }

    public bool IsPositionInsideBox(Vector3 worldPos)
    {
        if (triggerBox == null) return true;

        return triggerBox.bounds.Contains(worldPos);
    }

    private void StartPinch(Vector3 pos, Quaternion rot)
    {

        if (!IsPositionInsideBox(pos))
        {
            return;
        }

        GameObject closest = GetClosestStick(pos);

        if (closest != null)
        {
            selectedSticks.Add(closest);
            initPositions.Add(closest.transform.position);
            initRotations.Add(closest.transform.rotation);
            audioData.PlayGrabSound();
        }
    }

    private void ContinuePinch(Vector3 pos, Quaternion rot)
    {
        if (selectedSticks.Count == 0) return;

        GameObject currentStick = selectedSticks[selectedSticks.Count - 1];


        currentStick.transform.position = pos;
        currentStick.transform.rotation = rot;

        if (closestSpot != null)
            closestSpot.ResetGlow();

        closestSpot = GetClosestSpot(currentStick);
        if (closestSpot != null)
            closestSpot.SetGlow();
    }

    private void EndPinch(Vector3 pos, Quaternion rot)
    {
      

        if (selectedSticks.Count == 0) return;

        audioData.PlayUnGrabSound();
        GameObject currentStick = selectedSticks[selectedSticks.Count - 1];
        Spot currentSpot = GetClosestSpot(currentStick);

        if (closestSpot != null)
            closestSpot.ResetGlow();

        if (currentSpot != null)
        {
            if (IsStickSlotInAnySolution(currentStick, currentSpot))
            {
                HandleCorrectPlacement(currentStick, currentSpot);
                currentMoves++;

                UpdateMoveText();
            }
            else
            {
                ResetStick(currentStick, initPositions[selectedSticks.Count - 1], initRotations[selectedSticks.Count - 1]);
            }
        }
        else
        {
            ResetStick(currentStick, initPositions[selectedSticks.Count - 1], initRotations[selectedSticks.Count - 1]);
        }

        if (AllSticksPlacedCorrectly())
        {
            audioData.PlayLevelCompletedSound();
            levelData.MatchStickLevelSuccess();
            
        }

       
    }

    private void HandleCorrectPlacement(GameObject stick, Spot spot)
    {
        stick.transform.position = spot.transform.position;
        stick.transform.rotation = spot.transform.rotation;

        selectedSticks.Clear();
        initPositions.Clear();
        initRotations.Clear();
    }

    private void ResetStick(GameObject stick, Vector3 originalPos, Quaternion originalRot)
    {
        StartCoroutine(LerpToPosition(stick, originalPos, originalRot));
    }

    private IEnumerator LerpToPosition(GameObject stick, Vector3 pos, Quaternion rot)
    {
        float elapsed = 0f;
        Vector3 startPos = stick.transform.position;
        Quaternion startRot = stick.transform.rotation;

        while (elapsed < lerpDuration)
        {
            float t = elapsed / lerpDuration;
            stick.transform.position = Vector3.Lerp(startPos, pos, t);
            stick.transform.rotation = Quaternion.Slerp(startRot, rot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        stick.transform.position = pos;
        stick.transform.rotation = rot;

        selectedSticks.Clear();
        initPositions.Clear();
        initRotations.Clear();
    }

    private Spot GetClosestSpot(GameObject stick)
    {
        float minDist = float.MaxValue;
        Spot closest = null;

        foreach (Spot spot in slots)
        {
            float dist = Vector3.Distance(spot.transform.position, stick.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = spot;
            }
        }

        return closest;
    }

    private GameObject GetClosestStick(Vector3 pos)
    {
        float minDist = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject stick in matchSticks)
        {
            float dist = Vector3.Distance(stick.transform.position, pos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = stick;
            }
        }

        return closest;
    }

    private bool IsStickSlotInAnySolution(GameObject stick, Spot spot)
    {
        foreach (var solution in solutionPaths)
        {
            bool stickExists = solution.correctSticks.Contains(stick);
            bool spotExists = solution.correctSlots.Contains(spot);

            if (stickExists && spotExists)
                return true;
        }
        return false;
    }


    private bool AllSticksPlacedCorrectly()
    {
        foreach (var solution in solutionPaths)
        {
            int correctCount = 0;

            foreach (var stick in solution.correctSticks)
            {
                foreach (var slot in solution.correctSlots)
                {
                    if (Vector3.Distance(stick.transform.position, slot.transform.position) < 0.01f)
                    {
                        correctCount++;
                        break;
                    }
                }
            }


            if (correctCount == solution.correctSticks.Count)
                return true;
        }

        return false;
    }

    private void UpdateMoveText()
    {
        levelData.SetMatchStickMoves(currentMoves, totalMoves, hint);
    }

  
}


