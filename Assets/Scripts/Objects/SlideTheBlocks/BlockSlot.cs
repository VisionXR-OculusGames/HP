using UnityEngine;

public class BlockSlot : MonoBehaviour
{
    [Header(" Row And Column")]
    public int row;
    public int column;
    public bool isSlotOccupied = false;

    [Header(" Nearest Slots")]
    public BlockSlot left;
    public BlockSlot right;
    public BlockSlot up;
    public BlockSlot down;

    
}
