using UnityEngine;
using System;

public class CountdownManager : Manager
{
    [SerializeField] private float countdownDuration = 300f;

    private float remainingTime;
    private bool isPaused = false;
    private bool isRunning = false;

    [HideInInspector]
    public int TimeLeft;

    public float RemainingTime { get => remainingTime; set => remainingTime = value; }

    public event Action<float> OnTimeChanged;
    public event Action OnCountdownFinished;

    public override void Initialize()
    {
        remainingTime = countdownDuration;
        isRunning = true;
        isPaused = false;

        OnTimeChanged?.Invoke(remainingTime);
    }

    private void Update()
    {
        if (!isRunning || isPaused)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime < 0f)
            remainingTime = 0f;

        TimeLeft = Mathf.CeilToInt(remainingTime);

        OnTimeChanged?.Invoke(remainingTime);

        if (remainingTime <= 0f)
        {
            isRunning = false;
            OnCountdownFinished?.Invoke();
        }
    }

    public void Pause() => isPaused = true;
    public void Resume() => isPaused = false;

    public float GetRemainingTime() => remainingTime;
}