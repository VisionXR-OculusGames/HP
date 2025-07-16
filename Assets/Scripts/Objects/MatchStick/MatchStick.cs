using UnityEngine;

public class MatchStick : MonoBehaviour
{
    public enum MatchStickType
    {
        Movable,
        NonMovable
    }

    [Header("Matchstick Type")]
    public MatchStickType type = MatchStickType.NonMovable;

    [Header("Visual Elements")]
    public Renderer leftPoint;
    public Renderer rightPoint;
    public Renderer cylinder;

    [Header("Glow Settings")]
    public Color glowColor = Color.black;
    public Color normalColor = Color.black;
    [Range(0f, 1f)] public float glowAlpha = 0.5f;

    [Header("Point Colors")]
    public Color leftPointColor = Color.red;
    public Color rightPointColor = Color.red;

    [Header("Reset Glow Settings")]
    [Range(0f, 1f)] public float resetAlpha = 0.1f;

    private void OnEnable()
    {
        //ApplyCurrentTypeVisuals();
    }

    public void SetGlow()
    {
        Color c = new Color(glowColor.r, glowColor.g, glowColor.b, glowAlpha);
        ApplyColors(c, glowAlpha);
    }

    public void ResetGlow()
    {
        Color c = new Color(normalColor.r, normalColor.g, normalColor.b, resetAlpha);
        ApplyColors(c, resetAlpha);
    }

    private void ApplyColors(Color cylinderColor, float alpha)
    {
        // Cylinder uses glow/normal color
        if (cylinder) cylinder.material.color = cylinderColor;

        // Endpoints use their own custom color
        Color lp = new Color(leftPointColor.r, leftPointColor.g, leftPointColor.b, alpha);
        Color rp = new Color(rightPointColor.r, rightPointColor.g, rightPointColor.b, alpha);

        if (leftPoint) leftPoint.material.color = lp;
        if (rightPoint) rightPoint.material.color = rp;
    }

    private void ApplyCurrentTypeVisuals()
    {
        if (type == MatchStickType.Movable)
            SetGlow();
        else
            ResetGlow();
    }
}
