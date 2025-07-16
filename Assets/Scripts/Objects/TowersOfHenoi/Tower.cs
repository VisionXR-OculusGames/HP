using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public GameObject TopSlot;

    [Header("Slots (Bottom to Top)")]
    public List<Transform> slots; // Set in Inspector — slot[0] is bottom, slot[n] is top

    [Header("Current Disks")]
    public List<GameObject> disks = new List<GameObject>(); // Stack of disks currently on this tower

    // Get the top disk (last one in the list)
    public GameObject GetTopDisk()
    {
        if (disks.Count == 0) return null;
        return disks[disks.Count - 1];
    }

    // Get the next available slot to place a new disk
    public Transform GetNextAvailableSlot()
    {
        if (disks.Count >= slots.Count) return null;
        return slots[disks.Count];
    }

    // Try to place a new disk — returns true if placed, false if rejected
    public bool TryAddDisk(GameObject newDisk, int weight)
    {
        GameObject topDisk = GetTopDisk();
        int topWeight = topDisk ? topDisk.GetComponent<Disk>().weight : int.MaxValue;

        if (weight > topWeight)
        {
            Debug.Log("? Cannot place larger disk on a smaller one.");
            return false;
        }

        Transform slot = GetNextAvailableSlot();
        if (slot == null)
        {
            Debug.Log("? No available slot on tower.");
            return false;
        }

        // Position and parent the disk
        newDisk.transform.position = slot.position;
        newDisk.transform.rotation = slot.rotation;
       

        disks.Add(newDisk);
        return true;
    }

    // Remove and return the top disk
    public GameObject PopTopDisk()
    {
        if (disks.Count == 0) return null;

        GameObject top = disks[disks.Count - 1];
        disks.RemoveAt(disks.Count - 1);
        return top;
    }
}
