using UnityEngine;
using TMPro;

public class CountdownDisplay : MonoBehaviour
{
    private CountdownManager countdown;
    private TextMeshProUGUI timerText;

    private void Start()
    {
        countdown = FindAnyObjectByType<CountdownManager>();
        timerText = GetComponent<TextMeshProUGUI>();
        countdown.OnTimeChanged += UpdateDisplay;
    }

    private void UpdateDisplay(float time)
    {
        timerText.text = $"{(int)(time / 60)}:{(int)(time % 60):00}";
    }
}