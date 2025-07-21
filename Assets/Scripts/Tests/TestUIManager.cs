#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

using UnityEngine;
using UnityEngine.UI;

public class TestUIManager : MonoBehaviour
{
    [Header("Assign Buttons Here (Runtime)")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.digit1Key.wasPressedThisFrame && button1 != null)
            button1.onClick.Invoke();

        if (Keyboard.current.digit2Key.wasPressedThisFrame && button2 != null)
            button2.onClick.Invoke();

        if (Keyboard.current.digit3Key.wasPressedThisFrame && button3 != null)
            button3.onClick.Invoke();

        if (Keyboard.current.digit4Key.wasPressedThisFrame && button4 != null)
            button4.onClick.Invoke();
#else
        Debug.LogWarning("New Input System is not enabled. Please enable it in Project Settings.");
#endif
    }
}
