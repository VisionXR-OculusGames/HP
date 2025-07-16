using UnityEngine;

public class Spot : MonoBehaviour
{
    [Header("Visual Elements")]
    public Renderer leftPoint;
    public Renderer rightPoint;
    public Renderer cylinder;

    [Header("Glow Settings")]
    public Color glowColor = Color.black;
    [Range(0f, 1f)] public float glowAlpha = 0.5f;

    [Header("Reset Glow Settings")]
    [Range(0f, 1f)] public float resetAlpha = 0.1f;

    private void OnEnable()
    {
        ResetGlow();
    }

    public void SetGlow()
    {
        Color c = new Color(glowColor.r, glowColor.g, glowColor.b, glowAlpha);
        leftPoint.material.color = c;
        rightPoint.material.color = c;
        cylinder.material.color = c;
    }

    public void ResetGlow()
    {
        Color c = new Color(0, 0, 0, resetAlpha);
        leftPoint.material.color = c;
        rightPoint.material.color = c;
        cylinder.material.color = c;
    }
}
