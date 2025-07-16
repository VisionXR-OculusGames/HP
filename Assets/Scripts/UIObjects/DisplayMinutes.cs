using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMinutes : MonoBehaviour
{
    [Header("UI Text to Display Time")]
    public TMP_Text timeText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void OnEnable()
    {
        elapsedTime = 0f;
        isRunning = true;
    }

    private void OnDisable()
    {
        isRunning = false;
    }

    private void Update()
    {
        if (!isRunning || timeText == null) return;

        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetLevel()
    {
        elapsedTime = 0;
    }
}
