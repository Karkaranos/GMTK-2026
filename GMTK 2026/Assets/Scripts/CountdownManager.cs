using UnityEngine;
using System;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Collections;

public class CountdownManager : Manager
{
    [SerializeField] private float countdownDuration = 300f;
    [SerializeField] private Button funnyButton;

    private float remainingTime;
    private bool isPaused = false;
    private bool isRunning = false;

    [HideInInspector]
    public int TimeLeft;

    public float RemainingTime { get => remainingTime; set => remainingTime = value; }

    public event Action<float> OnTimeChanged;
    public static event Action OnCountdownFinished;

    public override void Initialize()
    {
        remainingTime = countdownDuration;
        isRunning = true;
        isPaused = false;

        ProgressBar.Cheat(1);

        OnTimeChanged?.Invoke(remainingTime);
        funnyButton.gameObject.SetActive(false);
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
            StartCoroutine(Done(false));
        }

        // this needs to be last in the function because return
        foreach (var part in FindAnyObjectByType<BuildingManager>().GetParts())
        {
            if (part.Value == null) return;
        }
        funnyButton.gameObject.SetActive(true);
    }

    private IEnumerator Done(bool launched)
    {
        isRunning = false;
        if (launched)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Launch);
            yield return new WaitForSeconds(10);
        }
        else
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Explosion);
            yield return new WaitForSeconds(1);
        }
        OnCountdownFinished?.Invoke();
        MenuBehavior.Instance.LoadGameScene(1);
    }

    public void Pause() => isPaused = true;
    public void Resume() => isPaused = false;

    public float GetRemainingTime() => remainingTime;

    public void ProceedToLaunch()
    {
        ProgressManager.INST.SkipAhead(Mathf.RoundToInt(countdownDuration));
        StartCoroutine(Done(true));
    }

    [Button]
    public void SkipToTheEnd()
    {
        remainingTime = 0.5f;
    }
    [Button]
    public void SkipProgressBars()
    {
        ProgressBar.Cheat(1000);
    }
    [Button]
    public void StopSkippingProgressBars()
    {
        ProgressBar.Cheat(1);
    }
}