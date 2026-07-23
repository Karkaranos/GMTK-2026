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
    private float scrollLocation;

    public float ScrollLocation
    {
        get { return scrollLocation; }
        set
        {
            scrollLocation = value;
            transform.position = baseLocation + Vector3.up * scrollLocation;
        }
    }

    private void Awake()
    {
        baseLocation = transform.position;
        scrollAction = InputSystem.actions.FindAction("Scroll");
        scrollAction.performed += HandleScrollPerformed;
    }

    private void OnDestroy()
    {
        scrollAction.performed -= HandleScrollPerformed;
    }

    private void HandleScrollPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ReadValue<Vector2>());
        ScrollLocation += scrollSpeed * obj.ReadValue<Vector2>().y;
    }

    private void Update()
    {
        Debug.Log(scrollAction.ReadValue<Vector2>());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + Vector3.down * scrollRange / 2, transform.position + Vector3.up * scrollRange / 2);
    }
}
