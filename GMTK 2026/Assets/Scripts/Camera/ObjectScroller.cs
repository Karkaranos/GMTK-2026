using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls scrolling up and down in the rocket scene.
/// </summary>
public class ObjectScroller : MonoBehaviour
{
    [SerializeField] private float scrollRange;
    [SerializeField] private float scrollSpeed;

    private InputAction scrollAction;
    private Vector3 baseLocation;
    [ShowNonSerializedField] private float scrollLocation;

    public float ScrollLocation
    {
        get { return scrollLocation; }
        set
        {
            scrollLocation = Mathf.Clamp(value, - scrollRange / 2, scrollRange / 2);
            transform.position = baseLocation + Vector3.up * scrollLocation;
        }
    }

    private void Awake()
    {
        baseLocation = transform.position;
        scrollAction = InputSystem.actions.FindAction("Scroll");
        scrollAction.Enable();
        scrollAction.performed += HandleScrollPerformed;
    }

    private void OnDestroy()
    {
        scrollAction.performed -= HandleScrollPerformed;
    }

    private void HandleScrollPerformed(InputAction.CallbackContext obj)
    {
        ScrollLocation -= scrollSpeed * obj.ReadValue<Vector2>().normalized.y;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + Vector3.down * scrollRange / 2, transform.position + Vector3.up * scrollRange / 2);
    }
}
