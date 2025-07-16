using com.VisionXR.Models;
using System.Collections.Generic;
using UnityEngine;

public class BlockLevel : MonoBehaviour
{

    [Header(" Scriptable Objects")]
    public LevelDataSO levelData;
    public InputDataSO inputData;
    public AudioDataSO audioData;

    [Header(" Block Objects")]
    public GameObject correctBlock;
    public GameObject destinationBlock;
    public List<GameObject> allBlocks;
    private GameObject levelParent;
    private Block currentBlock;

    [Header(" Display Data")]
    public int minMoves = 0;
    public float gridSize = 4;
    public int levelNo;
    private int moves = 0;

    // local variables
    private BlockSlot currentNeighbour;
    private float deltaX;
    private float deltaY;
    private bool isMoved = false;
    
    private BoxCollider triggerBox;
    private float snapThreshold = 0.2f;
    private float rayThreshold = 0.2f;
    private float rayCastOffset = 0.1f; // Time in seconds



    private void OnEnable()
    {

        inputData.PinchStartEvent += StartPinch;
        inputData.PinchEndEvent += EndPinch;
        inputData.PinchContinueEvent += ContinuePinch;
        Initialise();
    }

    private void OnDisable()
    {
        inputData.PinchStartEvent -= StartPinch;
        inputData.PinchEndEvent -= EndPinch;
        inputData.PinchContinueEvent -= ContinuePinch;
    }

    private void Initialise()
    {
        levelParent = gameObject;
        snapThreshold = 1.0f / gridSize;
        rayThreshold = transform.localScale.x / gridSize;
        rayCastOffset = rayThreshold / gridSize;
        triggerBox = GetComponent<BoxCollider>();
        moves = 0;
        levelData.SetMovesAndTotalMoves(moves,minMoves);
        levelData.SetLevel(levelNo);
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

        GameObject block = GetNearestBlock(pos);
        if (block != null)
        {
            currentBlock = block.GetComponent<Block>();
            audioData.PlayGrabSound();
            currentBlock.SetGlow();
        }
        isMoved = false;
    }

    private void ContinuePinch(Vector3 pos, Quaternion rot)
    {
        if (currentBlock == null) return;

        //if (!IsPositionInsideBox(pos)) return;

        Vector3 currentLocal = currentBlock.transform.localPosition;
        Vector3 localPos = levelParent.transform.InverseTransformPoint(pos);
        Vector3 offset = localPos - currentLocal;

      
        currentNeighbour = null;
    

        if (currentBlock.blockOrientation == Orientation.Horizontal)
        {
            deltaX = offset.x;
            

            Dir currentDir = deltaX > 0 ? Dir.Right : Dir.Left;
            currentNeighbour = (currentDir == Dir.Right) ? currentBlock.right?.right : currentBlock.left?.left;

            if (currentNeighbour != null && !currentNeighbour.isSlotOccupied && Mathf.Abs(deltaX) > snapThreshold)
            {
                
               if(currentBlock.GetSize() == 2)
                { 

                // Update slot references
                BlockSlot newLeftSlot = (currentDir == Dir.Right) ? currentBlock.right : currentBlock.left.left;
                BlockSlot newRightSlot = (currentDir == Dir.Right) ? currentBlock.right.right : currentBlock.left;

                UpdateSlots(currentBlock, newLeftSlot, newRightSlot);

                currentBlock.transform.position = (newLeftSlot.transform.position + newRightSlot.transform.position) / 2;
                 isMoved = true;
               }
               else
               {
                    // Update slot references
                    BlockSlot newLeftSlot = (currentDir == Dir.Right) ? currentBlock.middle : currentBlock.left.left;
                    BlockSlot newMiddleSlot = (currentDir == Dir.Right) ? currentBlock.right : currentBlock.left;
                    BlockSlot newRightSlot = (currentDir == Dir.Right) ? currentBlock.right.right : currentBlock.middle;

                    UpdateSlots(currentBlock, newLeftSlot, newMiddleSlot,newRightSlot);

                    currentBlock.transform.position = (newLeftSlot.transform.position + newRightSlot.transform.position) / 2;
                    isMoved = true;
                }
            }
            
        }
        else // Vertical movement
        {
            deltaY = offset.y;
            

            Dir currentDir = deltaY > 0 ? Dir.Up : Dir.Down;
            currentNeighbour = (currentDir == Dir.Up) ? currentBlock.left?.up : currentBlock.right?.down;

            if (currentNeighbour != null && !currentNeighbour.isSlotOccupied && Mathf.Abs(deltaY) > snapThreshold)
            {

                if (currentBlock.GetSize() == 2)
                {

                    BlockSlot newLeftSlot = (currentDir == Dir.Up) ? currentBlock.left.up : currentBlock.right;
                    BlockSlot newRightSlot = (currentDir == Dir.Up) ? currentBlock.left : currentBlock.right.down;

                    UpdateSlots(currentBlock, newLeftSlot, newRightSlot);

                    currentBlock.transform.position = (newLeftSlot.transform.position + newRightSlot.transform.position) / 2;
                    isMoved = true;
                }
                else
                {
                    BlockSlot newLeftSlot = (currentDir == Dir.Up) ? currentBlock.left.up : currentBlock.middle;
                    BlockSlot newMiddleSlot = (currentDir == Dir.Up) ? currentBlock.left : currentBlock.right;
                    BlockSlot newRightSlot = (currentDir == Dir.Up) ? currentBlock.middle : currentBlock.right.down;

                    UpdateSlots(currentBlock, newLeftSlot,newMiddleSlot, newRightSlot);

                    currentBlock.transform.position = (newLeftSlot.transform.position + newRightSlot.transform.position) / 2;
                    isMoved = true;
                }
            }
            
        }
    }

    private void UpdateSlots(Block block, BlockSlot newLeft, BlockSlot newRight)
    {
        // Clear old slot occupation
        if (block.left != null) block.left.isSlotOccupied = false;
        if (block.right != null) block.right.isSlotOccupied = false;

        // Assign new slots
        block.AddLeftSlot(newLeft);
        block.AddRightSlot(newRight);

        // Mark new slots as occupied
        if (newLeft != null) newLeft.isSlotOccupied = true;
        if (newRight != null) newRight.isSlotOccupied = true;
    }

    private void UpdateSlots(Block block, BlockSlot newLeft, BlockSlot newMiddle,BlockSlot newRight)
    {
        // Clear old slot occupation
        if (block.left != null) block.left.isSlotOccupied = false;
        if (block.right != null) block.right.isSlotOccupied = false;
        if (block.middle != null) block.middle.isSlotOccupied = false;

        // Assign new slots
        block.AddLeftSlot(newLeft);
        block.AddRightSlot(newRight);
        block.AddMiddleSlot(newMiddle);

        // Mark new slots as occupied
        if (newLeft != null) newLeft.isSlotOccupied = true;
        if (newRight != null) newRight.isSlotOccupied = true;
        if (newMiddle != null) newMiddle.isSlotOccupied = true;
    }

    private void EndPinch(Vector3 pos, Quaternion rot)
    {
        if (currentBlock == null) return;
        audioData.PlayUnGrabSound();

        if (IsPositionCloseToDestination())
        {
            levelData.BlockLevelSuccess();
            audioData.PlayLevelCompletedSound();
        }
        currentBlock.ResetGlow();

        if (isMoved)
        {
            moves += 1;
            levelData.SetMovesAndTotalMoves(moves, minMoves);
        }
        currentBlock = null;
    }

    private bool IsPositionCloseToDestination()
    {
        if (destinationBlock == null) return false;

        float distance = Vector3.Distance(currentBlock.transform.position, destinationBlock.transform.position);
        return distance < 0.05f;
    }



    private GameObject GetNearestBlock(Vector3 pos)
    {
        float minDist = float.MaxValue;
        GameObject nearest = null;

        foreach (GameObject block in allBlocks)
        {

            float dist = Vector3.Distance(pos, block.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = block;
            }

        }
        return nearest;
    }

   

    public enum Dir  {Left,Right,Up,Down }
}
