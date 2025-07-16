using com.VisionXR.Models;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SoltaireLevel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public InputDataSO inputData;
    public AudioDataSO audioData;
    public LevelDataSO levelData;

    [Header(" All Slots")]
    public List<SoltaireSlot> slots;


    // local variables
    private BoxCollider triggerBox;  // Assign this in inspector
    private SoltaireSlot selectedSlot;
    private GameObject selectedMarble;
    private Vector3 initPos;
    private Quaternion initRot;
    private List<SoltaireSlot> validJumpTargets = new List<SoltaireSlot>();
    public int minMarbles = 0;
    private int moves = 0;
    private int score = 0;


    private void OnEnable()
    {
        inputData.PinchStartEvent += StartPinch;
        inputData.PinchContinueEvent += ContinuePinch;
        inputData.PinchEndEvent += EndPinch;
        Initilaise();
    }

    private void OnDisable()
    {
        inputData.PinchStartEvent -= StartPinch;
        inputData.PinchContinueEvent -= ContinuePinch;
        inputData.PinchEndEvent -= EndPinch;
    }

    private void Initilaise()
    {
        triggerBox = GetComponent<BoxCollider>();
        moves = 0;
        score = 0;
        levelData.SetMoves(moves);
        levelData.SetScore(score);
        levelData.SetMinMarbles(minMarbles);
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

        selectedSlot = GetNearestOccupiedSlot(pos);
        if (selectedSlot == null) return;


            audioData.PlayGrabSound();
            selectedSlot.SetGlow();
            selectedMarble = selectedSlot.marble;
            initPos = selectedMarble.transform.position;
            initRot = selectedMarble.transform.rotation;

            validJumpTargets = GetValidJumpTargets(selectedSlot);
            HighlightSlots(validJumpTargets, true); // optional visual feedback
        
    }

    private void ContinuePinch(Vector3 pos, Quaternion rot)
    {
        if (selectedMarble != null)
        {
            selectedMarble.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }
    }

    private void EndPinch(Vector3 pos, Quaternion rot)
    {
        audioData.PlayUnGrabSound();

        if (selectedMarble == null) return;

        SoltaireSlot targetSlot = GetNearestValidTarget(pos);
        if (targetSlot != null)
        {
            SoltaireSlot jumpedSlot = GetJumpedSlot(selectedSlot, targetSlot);
            if (jumpedSlot != null && jumpedSlot.isOccupied)
            {
                // Move marble
                selectedMarble.transform.position = targetSlot.transform.position;
                targetSlot.marble = selectedMarble;
                targetSlot.isOccupied = true;

                // Clear original
                selectedSlot.marble = null;
                selectedSlot.isOccupied = false;

                // Remove jumped marble
                if (jumpedSlot.marble != null)
                {
                    jumpedSlot.marble.SetActive(false); // or move to outer slot
                    jumpedSlot.marble = null;
                    jumpedSlot.isOccupied = false;
                    moves++;
                    score++;
                    levelData.SetMoves(moves);
                    levelData.SetScore(score);
                }

         
            }
            else
            {
                ReturnMarble();
            }
        }
        else
        {
            ReturnMarble();
        }

        HighlightSlots(validJumpTargets, false);
        selectedSlot.ResetGlow();
        validJumpTargets.Clear();
        selectedMarble = null;
        selectedSlot = null;

        CheckGameComplete();
    }

    private void ReturnMarble()
    {
        selectedMarble.transform.position = initPos;
        selectedMarble.transform.rotation = initRot;
    }

    private SoltaireSlot GetNearestOccupiedSlot(Vector3 pos)
    {
        float minDist = float.MaxValue;
        SoltaireSlot nearest = null;

        foreach (SoltaireSlot slot in slots)
        {
            if (slot.isOccupied)
            {

                float dist = Vector3.Distance(pos, slot.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = slot;
                }
            }
        }
        return nearest;
    }

    private SoltaireSlot GetNearestValidTarget(Vector3 pos)
    {
        float minDistToValidTarget = float.MaxValue;
        SoltaireSlot nearestValidSlot = null;

        // First, check distance to original slot
        float distToSelectedSlot = Vector3.Distance(pos, selectedSlot.transform.position);

        // Then check all valid jump targets
        foreach (var slot in validJumpTargets)
        {
            float dist = Vector3.Distance(pos, slot.transform.position);
            if (dist < minDistToValidTarget)
            {
                minDistToValidTarget = dist;
                nearestValidSlot = slot;
            }
        }

        // If original slot is closer than any valid target, cancel the move
        if (distToSelectedSlot < minDistToValidTarget)
        {
            return null;
        }

        return nearestValidSlot;
    }


    private List<SoltaireSlot> GetValidJumpTargets(SoltaireSlot currentSlot)
    {
        List<SoltaireSlot> validTargets = new List<SoltaireSlot>();

        for (int i = 0;i < currentSlot.jumpableSlots.Count;i++)
        {
            SoltaireSlot midSlot = currentSlot.adjacentSlots[i];
            if (midSlot != null && midSlot.isOccupied && !currentSlot.jumpableSlots[i].isOccupied)
            {
                validTargets.Add(currentSlot.jumpableSlots[i]);
            }
        }

        return validTargets;
    }

    private SoltaireSlot GetJumpedSlot(SoltaireSlot from, SoltaireSlot to)
    {
        int j = 0;
        for (int i = 0;i<= from.jumpableSlots.Count;i++)
        {
            if(to.gameObject.GetInstanceID() == from.jumpableSlots[i].gameObject.GetInstanceID())
            {
                j = i;
                break;
            }
        }
          
        return from.adjacentSlots[j];
    }

    private void HighlightSlots(List<SoltaireSlot> slots, bool highlight)
    {
        // Optional: change material, outline, etc. based on `highlight`

        if (highlight)
        {
            foreach (var slot in slots)
            {
                slot.SetGlow();
            }
        }
        else
        {
            foreach (var slot in slots)
            {
                slot.ResetGlow();
            }
        }
    }

    private void CheckGameComplete()
    {
        foreach (var slot in slots)
        {
            if (!slot.isOccupied) continue;

            List<SoltaireSlot> possibleMoves = GetValidJumpTargets(slot);
            if (possibleMoves.Count > 0)
            {
                // At least one valid move exists ? game is not over
               // Debug.Log("Game is still ongoing.");
                return;
            }
        }

        audioData.PlayLevelCompletedSound();
        levelData.BrainvitaLevelSuccess();

    }

}
