using System.Collections.Generic;
using UnityEngine;

public class TangramSlot : MonoBehaviour
{
    [Header("Slot Info")]
    public string slotName;
    public List<TangramSubSlot> subSlots = new();
    public string shapeTag;
    public GameObject occupiedBy;

    private Dictionary<TangramSubSlot, Vector3> originalScales = new();
    private Dictionary<TangramSubSlot, Color> originalColors = new();

    public bool isOccupied => IsAnySubslotOccupied();

    private bool IsAnySubslotOccupied()
    {
        foreach (var sub in subSlots)
        {
            if (sub.isOccupied)
                return true;
        }
        return false;
    }

    public void MarkAllSubSlots(GameObject piece)
    {
        foreach (var sub in subSlots)
        {
            sub.isOccupied = true;
            sub.occupiedBy = piece;
        }
        occupiedBy = piece;
    }

    public void UnmarkAllSubSlots()
    {
        foreach (var sub in subSlots)
        {
            sub.isOccupied = false;
            sub.occupiedBy = null;
        }
        occupiedBy = null;
    }

    public void SetGlow()
    {
        foreach (var sub in subSlots)
        {
            if (!originalScales.ContainsKey(sub))
                originalScales[sub] = sub.transform.localScale;

            if (!originalColors.ContainsKey(sub))
            {
                Renderer rend = sub.GetComponent<Renderer>();
                if (rend != null && rend.material.HasProperty("_Color"))
                    originalColors[sub] = rend.material.color;
            }

            sub.transform.localScale = originalScales[sub] * 1.1f;

            Renderer renderer = sub.GetComponent<Renderer>();
            if (renderer != null && renderer.material.HasProperty("_Color"))
                renderer.material.color = Color.white;
        }
    }

    public void ResetGlow()
    {
        foreach (var sub in subSlots)
        {
            if (originalScales.TryGetValue(sub, out Vector3 original))
                sub.transform.localScale = original;
            else
                sub.transform.localScale = Vector3.one;

            Renderer renderer = sub.GetComponent<Renderer>();
            if (renderer != null && originalColors.TryGetValue(sub, out Color origColor))
                renderer.material.color = origColor;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (subSlots.Count == 0)
        {
            Debug.LogWarning($"TangramSlot '{gameObject.name}' has no subSlots assigned.", this);
        }
    }
#endif
}
