using UnityEngine;

public class TestObjectMovement : MonoBehaviour
{
    [Header("Target to Move")]
    public Transform targetObject;

    [Header("Movement Limits")]
    public float leftXLimit = -2f;
    public float rightXLimit = 2f;
    public float topYLimit = 2f;
    public float bottomYLimit = -2f;

    [Header("Z-Axis Lock")]
    public float fixedZ = 0f;

    [Header("Normalized Movement Scale")]
    public float movementRangeX = 2f;
    public float movementRangeY = 2f;

    private void Update()
    {
        if (targetObject == null) return;

        if (Input.GetMouseButton(0)) // Left mouse button
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 mousePos = Input.mousePosition;
            Vector2 offsetFromCenter = mousePos - screenCenter;

            float normalizedX = Mathf.Clamp(offsetFromCenter.x / screenCenter.x, -1f, 1f);
            float normalizedY = Mathf.Clamp(offsetFromCenter.y / screenCenter.y, -1f, 1f);

            Vector3 newPos = new Vector3(
                normalizedX * movementRangeX,
                normalizedY * movementRangeY,
                fixedZ
            );

            // Clamp to final limits
            newPos.x = Mathf.Clamp(newPos.x, leftXLimit, rightXLimit);
            newPos.y = Mathf.Clamp(newPos.y, bottomYLimit, topYLimit);

            targetObject.position = newPos;
        }
    }
}
