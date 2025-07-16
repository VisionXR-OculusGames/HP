using System.Collections.Generic;
using UnityEngine;

public class SoltaireSlot : MonoBehaviour
{
    [Header("Slot Status")]
    public bool isOccupied = false;

    [Header("Slot Connections")]
    public List<SoltaireSlot> adjacentSlots;    // For directional checking if needed
    public List<SoltaireSlot> jumpableSlots;    // Slots that can be jumped to (2 steps away)

    [Header("Slot Marble")]
    public GameObject marble;
    private Renderer slotRenderer;


    private void Start()
    {
        slotRenderer = GetComponent<Renderer>();
        ResetGlow();
    }

    public void SetGlow()
    {
        slotRenderer.material.color = new Color(0, 1, 0, 0.3f);
    }
    public void ResetGlow()
    {
        slotRenderer.material.color = new Color(0, 0, 0, 0);
    }

}
