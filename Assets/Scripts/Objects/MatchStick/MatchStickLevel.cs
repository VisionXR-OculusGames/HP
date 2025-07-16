using com.VisionXR.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStickLevel : MonoBehaviour
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
    public int NoOfMoves = 1;
    public string hint;

    // local
    [SerializeField] private float lerpDuration = 0.5f; // Time in seconds
    private BoxCollider triggerBox;
    private Coroutine resetRoutine = null;
    private GameObject selectedStick = null;
    private Vector3 matchStickInitPos;
    private Quaternion matchStickInitRot;
    private Spot closestSpot;

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
    }

    public bool IsPositionInsideBox(Vector3 worldPos)
    {
        return triggerBox.bounds.Contains(worldPos);
    }
    private void StartPinch(Vector3 pos, Quaternion rot)
    {
        if(!IsPositionInsideBox(pos))
        {
            return;
        }
       
        GameObject closest = GetClosestStick(pos);
       
        if (closest != null)
        {
            selectedStick = closest;
            
            matchStickInitPos = selectedStick.transform.position;
            matchStickInitRot = selectedStick.transform.rotation;

            audioData.PlayGrabSound();
        }
    }

    private void ContinuePinch(Vector3 pos, Quaternion rot)
    {

        if (selectedStick == null) return;

         Vector3 newPos = new Vector3(pos.x, pos.y, pos.z); // keep Z fixed
         selectedStick.transform.position = newPos;
         selectedStick.transform.rotation = rot;
        
        if (closestSpot != null)
        {
            closestSpot.ResetGlow();
        }

        closestSpot = GetClosestSpot(); 
        if (closestSpot != null)
        {
            closestSpot.SetGlow();
        }

    }

    private void EndPinch(Vector3 pos, Quaternion rot)
    {
       
        if (selectedStick == null) return;

        audioData.PlayUnGrabSound();

        closestSpot = GetClosestSpot();

        if (closestSpot != null)
        {
            if (IsStickSlotInAnySolution(selectedStick,closestSpot))
            {
                HandleCorrectPlacement(closestSpot);
                audioData.PlayLevelCompletedSound();
            }
            else
            {
                
                ResetToOriginalPosition();
            }

            closestSpot.ResetGlow();
        }
        else
        {
          
            ResetToOriginalPosition();
        }

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

    private void HandleCorrectPlacement(Spot spot)
    {
        selectedStick.transform.position = spot.transform.position;
        selectedStick.transform.rotation = spot.transform.rotation;
        levelData.MatchStickLevelSuccess();
        
    }

    private void ResetToOriginalPosition()
    {
        
        if (resetRoutine != null)
        {
            StopCoroutine(resetRoutine);
            resetRoutine = null;
        }

        resetRoutine = StartCoroutine(LerpToOriginalPosition());
    }

    private IEnumerator LerpToOriginalPosition()
    {
        float elapsed = 0f;
        Vector3 startPos = selectedStick.transform.position;
        Quaternion startRot = selectedStick.transform.rotation;

        while (elapsed < lerpDuration)
        {
            float t = elapsed / lerpDuration;
            selectedStick.transform.position = Vector3.Lerp(startPos, matchStickInitPos, t);
            selectedStick.transform.rotation = Quaternion.Slerp(startRot, matchStickInitRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        selectedStick.transform.position = matchStickInitPos;
        selectedStick.transform.rotation = matchStickInitRot;

        resetRoutine = null;
        selectedStick = null;
    }


    private Spot GetClosestSpot()
    {
        float minDist = float.MaxValue;
        Spot closestSpot = null;

        foreach (Spot spot in slots)
        {
            float dist = Vector3.Distance(spot.transform.position, selectedStick.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestSpot = spot;
            }
        }

        return closestSpot;
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

  
}


[System.Serializable]
public class MatchStickSolution
{
    public List<GameObject> correctSticks;
    public List<Spot> correctSlots;
}