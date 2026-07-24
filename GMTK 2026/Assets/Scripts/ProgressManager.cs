using NaughtyAttributes;
using System.Collections;
using UnityEngine;
public class ProgressManager : Manager
{
    public static ProgressManager INST;

    [SerializeField, BoxGroup("Timer")]
    private float _timer;
    [SerializeField, MinValue(0), BoxGroup("Timer"), Tooltip("How ofter the timer updates.")]
    private float _timerUpdateInterval = 0.01f;

    [SerializeField, MinValue(0)]
    private float _tick = 1;  

    [ReadOnly]
    private float progress;
    [ReadOnly]
    private float distanceFlown; //max 1620
    [ReadOnly]
    private float shipQuality; //min 3, max 9

    public override void Initialize()
    {
        if (INST == null)
            INST = this;
        else if (INST != this)
            Debug.LogError("There are multiple instances of ProgressManager. There can only be one.");
    }

    private void Start()
    {
        StartCoroutine(Tick());
        StartCoroutine(TimerCD());
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(_tick);
            CalculateProgressPerSecond();
        }
    }

    private IEnumerator TimerCD()
    {
        while (_timer > 0)
        {
            yield return new WaitForSeconds(_timerUpdateInterval);
            _timer -= _timerUpdateInterval;
        }
        Debug.Log("TIME'S OUT");
    }

    private void AddProgress(float amount)
    {
        progress += progress;
        Mathf.Clamp(progress, 0, 100);
    }

    private void CalculateProgressPerSecond()
    {

    }

    private void CalculateShipQuality()
    {
       // shipQuality = 
    }

    private void CalculateDistanceFlown()
    {
        distanceFlown = shipQuality * progress; //* modifiers
    }
}
