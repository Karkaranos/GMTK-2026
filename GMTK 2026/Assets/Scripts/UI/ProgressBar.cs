using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider slider;

    public enum FillType { Timer, Hold };
    [SerializeField] private FillType _fillType;
    [SerializeField, ShowIf("IsTimer")] private bool _resetOnEnable;
    [SerializeField, ShowIf("IsTimer")] private bool _beginOnEnable;
    private bool IsTimer() => _fillType == FillType.Timer;

    [SerializeField, Tooltip("# of seconds long")] private float _internalWidth;
    [SerializeField, Tooltip("How much it fills each second.")] private float fillRate = 1;
    private float fillAmount;

    private bool filling;
    private Coroutine fillCoroutine;
    public Action OnBarEnable;
    public Action OnBarFinish;

    public float FillRate
    {
        get => fillRate;
        set => FillRate = Mathf.Max(value, 0);
    }

    void Awake()
    {
        slider = GetComponentInChildren<Slider>();
        if (slider == null)
        {
            Debug.LogWarning(gameObject.name + " has ProgressBar but no slider");
            Destroy(this);
            return;
        }

        slider.minValue = 0;
        slider.maxValue = _internalWidth;
        slider.value = 0;

        if (_fillType == FillType.Timer)
        {
            if (_resetOnEnable) OnBarEnable += CancelFill;
            if (_beginOnEnable) OnBarEnable += BeginFill;
        }
    }

    private void OnEnable()
    {
        OnBarEnable?.Invoke();
    }

    private void OnDisable()
    {
        PauseFill();
    }

    [Button]
    public void BeginFill()
    {
        if (fillCoroutine != null || slider == null) return;
        filling = true;
        fillCoroutine = StartCoroutine(Fill());
    }

    [Button]
    public void PauseFill()
    {
        if (fillCoroutine == null || slider == null) return;
        StopCoroutine(fillCoroutine);
        fillCoroutine = null;
        filling = false;
    }

    [Button]
    public void CancelFill()
    {
        if (slider == null) return;
        PauseFill();
        fillAmount = 0;
        slider.value = 0;
    }

    private IEnumerator Fill()
    {
        while (fillAmount < _internalWidth)
        {
            yield return null;
            fillAmount += Time.deltaTime * fillRate;
            slider.value = fillAmount;

            if (fillAmount > _internalWidth)
            {
                OnBarFinish?.Invoke();
                if (filling) PauseFill();
            }
        }
    }
}
