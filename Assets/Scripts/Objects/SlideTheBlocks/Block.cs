using UnityEngine;

public class Block : MonoBehaviour
{
    public MeshRenderer blockRender;
    public Orientation blockOrientation;

    [Header(" Slots ")]
    public BlockSlot left;
    public BlockSlot right;
    public BlockSlot middle;

    public void SetGlow()
    {
        blockRender.enabled = true;
    }
    public void ResetGlow()
    {
        blockRender.enabled = false;
    }

    public void AddLeftSlot(BlockSlot slot)
    {
        left = slot;
    }

    public void AddRightSlot(BlockSlot slot)
    {
        right = slot;
    }

    public void AddMiddleSlot(BlockSlot slot)
    {
        middle = slot;
    }

    public int GetSize()
    {
        if(middle == null)
        {
            return 2;
        }

        return 3;
    }
    public void ClearSlots()
    {
        left = null;
        right = null;
        middle = null;
    }
}

public enum Orientation { Horizontal, Vertical }