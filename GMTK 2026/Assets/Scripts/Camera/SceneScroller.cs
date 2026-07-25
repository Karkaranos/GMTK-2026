using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Controls scrolling up and down in the rocket scene.
/// </summary>
public class SceneScroller : MonoBehaviour
{
    private const float LERP_LEEWAY = 0.01f;

    [SerializeField] private float scrollRange;
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Scrollbar sceneScrollbar;

    private InputAction scrollAction;
    private Vector3 baseLocation;
    [ShowNonSerializedField] private float targetScrollLocation;
    private float scrollLocation;

    private SingletonCoroutine lerpRoutine;

    public float ScrollLocation
    {
        get {  return scrollLocation; }
        set
        {
            scrollLocation = value;
            transform.position = baseLocation + Vector3.up * scrollLocation;
        }
    }
    public float TargetScrollLocation
    {
        get { return targetScrollLocation; }
        set
        {
            targetScrollLocation = Mathf.Clamp(value, - scrollRange / 2, scrollRange / 2);
            if (Mathf.Abs(targetScrollLocation - scrollLocation) > LERP_LEEWAY)
            {
                lerpRoutine.StartCoroutineIgnore(LerpToTargetPos());
            }
        }
    }

    private void Awake()
    {
        lerpRoutine = new SingletonCoroutine(this);
        baseLocation = transform.position;
        scrollAction = InputSystem.actions.FindAction("Scroll");
        scrollAction.Enable();
    }

    private void OnEnable()
    {
        scrollAction.performed += HandleScrollPerformed;
    }

    private void OnDisable()
    {
        scrollAction.performed -= HandleScrollPerformed;
    }

    private void HandleScrollPerformed(InputAction.CallbackContext obj)
    {
        TargetScrollLocation -= scrollSpeed * obj.ReadValue<Vector2>().normalized.y;
    }

    private IEnumerator LerpToTargetPos()
    {
        while(Mathf.Abs(targetScrollLocation - scrollLocation) > LERP_LEEWAY)
        {
            float step = 1 - Mathf.Pow(0.5f, Time.deltaTime * lerpSpeed);
            ScrollLocation = Mathf.Lerp(scrollLocation, targetScrollLocation, step);
            UpdateScrollbar();

            yield return null;
        }
        ScrollLocation = targetScrollLocation;
        UpdateScrollbar();
    }

    private void UpdateScrollbar()
    {
        if (sceneScrollbar != null)
        {
            sceneScrollbar.SetValueWithoutNotify((scrollLocation + (scrollRange / 2)) / scrollRange);
        }
    }

    public void SetScrollNormalized(float normalizedValue)
    {
        lerpRoutine.StopCoroutine();
        ScrollLocation = (normalizedValue * scrollRange) - (scrollRange / 2);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + Vector3.down * scrollRange / 2, transform.position + Vector3.up * scrollRange / 2);
    }
}
