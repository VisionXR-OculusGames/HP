using com.VisionXR.Models;
using System.Collections.Generic;
using UnityEngine;

public class HenoiLevel : MonoBehaviour
{
    [Header(" Scriptable Objects")]
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public AudioDataSO audioData;

    [Header(" Game Objects")]
    public Tower correctTower;
    public List<Tower> towers;
    public List<Disk> disks;
    private BoxCollider triggerBox;

    // Local variables
    private int moves = 0;
    public int level ;
    private Tower closestTower;
    private GameObject selectedDisk = null;
    private Vector3 diskInitPos;
    private Quaternion diskInitRot;

    private void OnEnable()
    {
       
        inputData.PinchStartEvent += StartPinch;
        inputData.PinchEndEvent += EndPinch;
        inputData.PinchContinueEvent += ContinuePinch;

        Initilaise();
    }


    private void OnDisable()
    {
        inputData.PinchStartEvent -= StartPinch;
        inputData.PinchEndEvent -= EndPinch;
        inputData.PinchContinueEvent -= ContinuePinch;
    }

    private void Initilaise()
    {
        triggerBox = GetComponent<BoxCollider>();
        moves = 0;
        levelData.SetMoves(moves);
        levelData.SetLevel(level);
    }
    public bool IsPositionInsideBox(Vector3 worldPos)
    {
        return triggerBox.bounds.Contains(worldPos);
    }

    private void StartPinch(Vector3 pos, Quaternion rot)
    {
        if (!IsPositionInsideBox(pos))
        {
            return;
        }

        closestTower = GetClosestTower(pos);

        if(closestTower != null)
        {
            selectedDisk = closestTower.GetTopDisk();

            if(selectedDisk != null)
            {

                diskInitPos = selectedDisk.transform.position;
                diskInitRot = selectedDisk.transform.rotation;
                closestTower.disks.Remove(selectedDisk);
                audioData.PlayGrabSound();

                selectedDisk.transform.position = closestTower.TopSlot.transform.position;
            }
        }
    }

    private void ContinuePinch(Vector3 pos, Quaternion rot)
    {
        if (selectedDisk == null) return;

        GameObject topSlot = closestTower.TopSlot;

        // Convert world pos to local position relative to topSlot
        Vector3 localHandPos = transform.InverseTransformPoint(pos);
        Vector3 localSlotPos = transform.InverseTransformPoint(topSlot.transform.position);

        Vector3 offset = (localHandPos - localSlotPos);

        // Move the disk along the topSlot's local X direction
        selectedDisk.transform.position = topSlot.transform.position + transform.right * offset.x;

       // selectedDisk.transform.rotation = rot;
    }

    private void EndPinch(Vector3 pos, Quaternion rot)
    {
        if (selectedDisk == null) return;

        moves++;
        levelData.SetMoves(moves);

        Tower targetTower = GetClosestTower(pos);
        Disk diskData = selectedDisk.GetComponent<Disk>();

        if (targetTower != null && diskData != null)
        {
            GameObject topDisk = targetTower.GetTopDisk();
            int topWeight = topDisk ? topDisk.GetComponent<Disk>().weight : int.MaxValue;

            if (diskData.weight < topWeight)
            {
                // Valid move
                bool added = targetTower.TryAddDisk(selectedDisk, diskData.weight);
                if (added)
                {
                    audioData.PlayUnGrabSound();
                    CheckWinCondition();
                    selectedDisk = null;
                    return;
                }
            }
        }

        // Invalid move ? return
        selectedDisk.transform.position = diskInitPos;
        selectedDisk.transform.rotation = diskInitRot;
        closestTower.TryAddDisk(selectedDisk, selectedDisk.GetComponent<Disk>().weight);
        selectedDisk = null;
    }

    private void CheckWinCondition()
    {
        if (correctTower.disks.Count != disks.Count)
            return;

        for (int i = 0; i < correctTower.disks.Count - 1; i++)
        {
            int currentWeight = correctTower.disks[i].GetComponent<Disk>().weight;
            int nextWeight = correctTower.disks[i + 1].GetComponent<Disk>().weight;

            if (currentWeight < nextWeight)
            {
                // Invalid order: larger disk is below smaller one
                return;
            }
        }

        Debug.Log("?? All disks placed in correct order on the correct tower!");
        levelData.HenoiLevelSuccess();
        audioData.PlayLevelCompletedSound();
    }


    private Tower GetClosestTower(Vector3 pos)
    {
        float minDist = float.MaxValue;
        Tower closest = null;


        foreach (Tower t in towers)
        {
            float dist = Vector3.Distance(t.transform.position, pos);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }

}
