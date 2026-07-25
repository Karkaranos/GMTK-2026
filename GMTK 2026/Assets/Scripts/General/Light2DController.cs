using MarUtility;
using NaughtyAttributes;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class Light2DController : MonoBehaviour
{
    private Light2D light;

    [SerializeField, Label("Lerp Data Inensity")]
    private LerpData _ldIntensity;

    [SerializeField, BoxGroup("Simulate")]
    private float _simIntensity;

    private float lStartIntensity;
    private float lEndIntensity;
    private float lTimeIntensity;

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        light = GetComponent<Light2D>();
    }

    public void BeginLerpIntensity(float end)
        =>BeginLerpIntensity(light.intensity, end);
    public void BeginLerpIntensity(float start, float end)
    {
        lStartIntensity = start;
        lEndIntensity = end;
        StartCoroutine(LerpIntervalIntensity());
    }

    private IEnumerator LerpIntervalIntensity()
    {
        lTimeIntensity = 0;
        light.intensity = lStartIntensity;
        _ldIntensity.OnStart.Invoke();

        while (lTimeIntensity < _ldIntensity.Duration)
        {
            light.intensity = Mathf.Lerp(lStartIntensity, lEndIntensity, lTimeIntensity / _ldIntensity.Duration);
            lTimeIntensity += Time.deltaTime;
            _ldIntensity.OnBody.Invoke();
            yield return null;
        }
        light.intensity = lEndIntensity;
        _ldIntensity.OnEnd.Invoke();
    }

    [Button]
    private void Simulate()
    {
        if (!EditorApplication.isPlaying)
        {
            DebugMessages.SimulationPlaytestOnly();
            return;
        }
        BeginLerpIntensity(_simIntensity);
    }
}
